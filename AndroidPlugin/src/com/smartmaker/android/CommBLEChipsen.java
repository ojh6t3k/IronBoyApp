package com.smartmaker.android;


import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattService;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.ServiceConnection;
import android.os.IBinder;
import android.util.Log;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Set;

import com.chipsen.bleservice.BluetoothLeService;
import com.chipsen.bleservice.SampleGattAttributes;


public class CommBLEChipsen
{
	private static CommBLEChipsen _instance = null;
	private Context _context;
	private BluetoothLeService _bluetoothLeService;
	private BluetoothGattCharacteristic _uartRead;
	private BluetoothGattCharacteristic _uartWrite;
	private BluetoothAdapter _btAdapter;
	private boolean _isOpen = false;
	private List<Byte> _rcvBuffer = new ArrayList<Byte>();
	
	public static CommBLEChipsen GetInstance()
    {
		Log.d("CommBLEChipsen", "GetInstance");
		
        if(_instance == null)
        	_instance = new CommBLEChipsen();        
        
        return _instance;
    }
	
	public synchronized void StartActivity(Context context)
    {
		Log.d("CommSocket", "StartActivity");
		
        _context = context;
        
        _btAdapter = BluetoothAdapter.getDefaultAdapter();
        
        // BLE ���� ����
        Intent gattServiceIntent = new Intent(_context, BluetoothLeService.class);
        _context.bindService(gattServiceIntent, _serviceConnection, Context.BIND_AUTO_CREATE);
        
        // Event �ڵ鷯 ���
        IntentFilter intentFilter = new IntentFilter();
        intentFilter.addAction(BluetoothLeService.ACTION_DATA_AVAILABLE);
        intentFilter.addAction(BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED);
        _context.registerReceiver(_broadcastReceiver, intentFilter);
    }
	
	public synchronized void StopActivity()
	{
		Log.d("CommSocket", "StopActivity");
		
		_context.unregisterReceiver(_broadcastReceiver);
		_context.unbindService(_serviceConnection);
		_bluetoothLeService = null;
	}
	
	public synchronized String[] GetBondedBluetooth()
    {
		Log.d("CommSocket", "GetBondedBluetooth");
		
        List<String> btDevices = new ArrayList<String>();
        
        if(_btAdapter != null && _bluetoothLeService != null)
        {
        	if(_btAdapter.isEnabled())
            {
            	try
                {
                    Set<BluetoothDevice> bondedDevices = _btAdapter.getBondedDevices();
                    if (bondedDevices.size() > 0)
                    {
                        for (BluetoothDevice bd : bondedDevices)
                        	btDevices.add(String.format("%s,%s", bd.getName(), bd.getAddress()));
                    }
                }
                catch (Exception e)
                {
                }
            }
            else
                _context.startActivity(new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE));
        
        }        
        
        return btDevices.toArray(new String[btDevices.size()]);
    }
	
	public synchronized boolean Open(String address)
	{
		Log.d("CommBLEChipsen", "Open");
		
		if(_btAdapter == null || _bluetoothLeService == null)
			return false;
		
		if(!_btAdapter.isEnabled())
		{
			_context.startActivity(new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE));
			return false;
		}
		
		if(!_isOpen)
		{
			if(_bluetoothLeService != null)
			{
				if(_bluetoothLeService.connect(address))
		        	_isOpen = true;
			}
			else
				Log.d("CommBLEChipsen", "BluetoothService not avaliable!");
		}
		else
			Log.d("CommBLEChipsen", "Already Open");


        return _isOpen;
	}
	
	public synchronized void Close()
	{
		Log.d("CommBLEChipsen", "Close");
		
		if(_isOpen)
		{
			if(_bluetoothLeService != null)
			{
				_isOpen = false;
				_bluetoothLeService.disconnect();
				_rcvBuffer.clear();
			}
			else
				Log.d("CommBLEChipsen", "BluetoothService not avaliable!");
		}
		else
			Log.d("CommBLEChipsen", "Already closed");
	}
	
	public boolean IsOpen()
	{
		return _isOpen;
	}
	
	public synchronized boolean Write(byte[] data)
	{
		if(_isOpen)
		{
			try
			{
	    		_bluetoothLeService.writeCharacteristic_NO_RESPONSE(_uartWrite, data);
			}
			catch (Exception e)
			{
				Log.d("CommBLEChipsen", "Write error");
				Close();
			}
		}
		
		return _isOpen;
	}
	
	public synchronized byte[] Read()
	{
		if(_isOpen)
		{
			try
			{
				byte[] data = new byte[_rcvBuffer.size()];
				for(int i=0; i<data.length; i++)
				{
					data[i] = _rcvBuffer.get(0);
					_rcvBuffer.remove(0);
				}
				
				return data;
			}
			catch (Exception e)
			{
				Log.d("CommBLEChipsen", "Read error");
				Close();
			}
		}
		
		return null;
	}
	
	// BLE���� ���� 
    private final ServiceConnection _serviceConnection = new ServiceConnection()
    {
        @Override
        public void onServiceConnected(ComponentName componentName, IBinder service)
        {
            Log.d("CommBLEChipsen", "Service Connected");
            
            _bluetoothLeService = ((BluetoothLeService.LocalBinder) service).getService();
            if(_bluetoothLeService.initialize())
            	findCharacteristic();
            else
            {
            	Log.d("CommBLEChipsen", "Service Failed to initialized");
            	_bluetoothLeService = null;
            }
         }

        @Override
        public void onServiceDisconnected(ComponentName componentName)
        {
        	Log.d("CommBLEChipsen", "Service Disconnected");
        	
        	Close();
        }
    };
    
    //BLE ���񽺸� �ڵ鸵 �� �� �ִ� �̺�Ʈ 
    // ACTION_GATT_CONNECTED: GATT���� ����
    // ACTION_GATT_DISCONNECTED: GATT���� ����ȵ�.
    // ACTION_GATT_SERVICES_DISCOVERED: GATT���� ã��
    // ACTION_DATA_AVAILABLE:  �����͸� �аų� BLE �۵��� �˸��� BLE ��ġ���� ������ ���� �� �ִ�. 
    private final BroadcastReceiver _broadcastReceiver = new BroadcastReceiver()
    {
        @Override
        public void onReceive(Context context, Intent intent)
        {
            final String action = intent.getAction();
            if (BluetoothLeService.ACTION_DATA_AVAILABLE.equals(action))
            {
            	byte[] data = intent.getByteArrayExtra(BluetoothLeService.EXTRA_DATA_RAW); 		//�����Ͱ� ������ �б�
            	String data_string = intent.getStringExtra(BluetoothLeService.EXTRA_DATA_STRING);	//�����Ͱ� ��Ʈ������ ��ȯ�ؼ� �б�
            	String uudi_data = intent.getStringExtra(BluetoothLeService.UUID_STRING);	//UUID�� ��Ʈ������ ��ȯ�ؼ� �б�
            	
            	// TODO Auto-generated method stub
        		if(SampleGattAttributes.UART_READ_UUID.equals(uudi_data))
        		{
        			for(int i=0; i<data.length; i++)
        				_rcvBuffer.add(new Byte(data[i]));
        		}
            }
            else if (BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED.equals(action))
            {
                // Show all the supported services and characteristics on the user interface.
            	//���񽺰� ������ �������̽��� �����Ѵ�.
            	findCharacteristic();
            }            
        }
    };
    
    private void findCharacteristic()
    {
    	// Find BLE112 service for writing to
        List<BluetoothGattService> gattServices = _bluetoothLeService.getSupportedGattServices();
        
        if (gattServices == null)
        	return;
        
        String uuid = null;
       
        // Loops through available GATT Services.
        //������ GATT ���񽺸� ���� UUID�� ã�´�. 
        for (BluetoothGattService gattService : gattServices)
        {
            List<BluetoothGattCharacteristic> gattCharacteristics = gattService.getCharacteristics();
            // Loops through available Characteristics.
            for (BluetoothGattCharacteristic gattCharacteristic : gattCharacteristics)
            {
                uuid = gattCharacteristic.getUuid().toString();
                if (SampleGattAttributes.UART_READ_UUID.equals(uuid))
                {
                	_uartRead = gattCharacteristic;
                	_bluetoothLeService.setCharacteristicNotification( _uartRead, true);// Notification on ����
                }
                if (SampleGattAttributes.UART_WRITE_UUID.equals(uuid))
                	_uartWrite = gattCharacteristic;
            }
        }
	}
}
