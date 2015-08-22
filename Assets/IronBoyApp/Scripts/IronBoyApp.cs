﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;


public class IronBoyApp : MonoBehaviour
{
	public CommObject commObject;
	public float timeoutSec = 5f;

	public UnityEvent OnConnected;
	public UnityEvent OnConnectionFailed;
	public UnityEvent OnDisconnected;

	private bool _connected = false;
	private bool _processRx = false;
	private float _time = 0f;
	private int _batteryRemaining;
	private int _rollAngle;
	private int _pitchAngle;
	private float _motionSpeed = 1f;
	private bool _balance = true;
	private bool _autoRecovery = true;
	private bool _torque = true;
	private List<byte> _rxDataBytes = new List<byte>();


	// Use this for initialization
	void Start ()
	{
		if(commObject != null)
		{
			commObject.OnOpened += CommOpenEventHandler;
			commObject.OnOpenFailed += CommOpenFailEventHandler;
			commObject.OnErrorClosed += CommErrorCloseEventHandler;
		}	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_connected == true)
		{
			if(_processRx == true)
			{
				byte[] readBytes = commObject.Read();
				if(readBytes != null)
				{
					for(int i=0; i<readBytes.Length; i++)
						_rxDataBytes.Add(readBytes[i]);

					// find header
					while(_rxDataBytes.Count > 2)
					{
						if(_rxDataBytes[0] == 0xff && _rxDataBytes[1] == 0xff)
							break;

						_rxDataBytes.RemoveAt(0);
					}

					// check feedback
					if(_rxDataBytes.Count >= 4)
					{
						if(_rxDataBytes[3] == 0x01)
						{
							if(_rxDataBytes.Count >= 6)
							{
								if(Checksum(6) == true)
								{
								}

								DebugRxPacket(6, "Type1");
								_rxDataBytes.RemoveRange(0, 6);
								_processRx = false;
							}
						}
						else if(_rxDataBytes[3] == 0x07)
						{
							if(_rxDataBytes.Count >= 15)
							{
								if(Checksum(15) == true)
								{
									_batteryRemaining = _rxDataBytes[5];
									_pitchAngle = (int)(_rxDataBytes[7] << 8 + _rxDataBytes[6]);
									_rollAngle = (int)(_rxDataBytes[9] << 8 + _rxDataBytes[8]);
								}

								DebugRxPacket(15, "Type2");
								_rxDataBytes.RemoveRange(0, 15);
								_processRx = false;
							}
						}
					}

					TimeoutReset();
				}

				// Check timeout
				if(_time > timeoutSec) // wait until timeout seconds
					ErrorDisconnect();
				else
					_time += Time.deltaTime;
			}
		}
	}

	public void TestPacket()
	{
		RunMotion(254, 10, 1f, 1f, 1f, 1f);
	}

	private void DebugRxPacket(int count, string text)
	{
		string debugText = text + ": ";
		for(int i=0; i<count; i++)
		{
			debugText += _rxDataBytes[i].ToString("X");
			debugText += " ";
		}
		Debug.Log(debugText);
	}

	public void Ping(int robotID)
	{
		if(_processRx == true)
			return;
		
		byte[] packet = new byte[7];
		
		packet[0] = 0xff;
		packet[1] = 0xff;
		packet[2] = (byte)robotID;
		packet[3] = 0x7f; // Instruction
		packet[4] = 0x01; // Feedback
		packet[5] = 0x00; // Transmit tag
		byte checksum = 0;
		for(int i=2; i<(packet.Length - 2); i++)
			checksum += packet[i];
		packet[6] = checksum;
		
		commObject.Write(packet);
		_processRx = true;
	}

	public void RunMotion(int robotID, int motionIndex, float lVertical, float lHorizontal, float rVertical, float rHorizontal)
	{
		if(_processRx == true)
			return;

		byte[] packet = new byte[15];

		packet[0] = 0xff;
		packet[1] = 0xff;
		packet[2] = (byte)robotID;
		packet[3] = 0x01; // Instruction
		packet[4] = 0x00; // Transmit tag
		packet[5] = (byte)motionIndex;
		int iValue = (int)(lVertical * 127f);
		packet[6] = (byte)iValue;
		iValue = (int)(lHorizontal * 127f);
		packet[7] = (byte)iValue;
		iValue = (int)(rVertical * 127f);
		packet[8] = (byte)iValue;
		iValue = (int)(rHorizontal * 127f);
		packet[9] = (byte)iValue;
		packet[10] = 0xff;
		packet[11] = 0xff;
		packet[12] = 0xff;
		packet[13] = 0xff;
		byte checksum = 0;
		for(int i=2; i<(packet.Length - 2); i++)
			checksum += packet[i];
		packet[14] = checksum;

		commObject.Write(packet);
		_processRx = true;
	}

	public void SetFunction(int robotID, float motionSpeed, bool balance, bool autoRecovery, bool torque)
	{
		if(_processRx == true)
			return;

		_motionSpeed = Mathf.Clamp(motionSpeed, 0.5f, 1.5f);
		_balance = balance;
		_autoRecovery = autoRecovery;
		_torque = torque;

		byte[] packet = new byte[10];
		
		packet[0] = 0xff;
		packet[1] = 0xff;
		packet[2] = (byte)robotID;
		packet[3] = 0x66; // Instruction
		packet[4] = 0x00; // Transmit tag
		int iValue = (int)(_motionSpeed * 100f);
		packet[5] = (byte)iValue;
		packet[6] = 0xff;
		packet[7] = 0xff;
		byte flag = 0x00;
		if(_balance == true)
			flag |= 0x01;
		else
			flag &= 0xfe;
		if(_autoRecovery == true)
			flag |= 0x02;
		else
			flag &= 0xfd;
		if(_torque == true)
			flag |= 0x80;
		else
			flag &= 0x7f;
		packet[8] = flag;
		byte checksum = 0;
		for(int i=2; i<(packet.Length - 2); i++)
			checksum += packet[i];
		packet[9] = checksum;
		
		commObject.Write(packet);
		_processRx = true;
	}

	private bool Checksum(int length)
	{
		byte checksum = 0;
		for(int i=2; i<(length - 2); i++)
			checksum += _rxDataBytes[i];

		if(_rxDataBytes[length - 1] == checksum)
			return true;
		else
			return false;
	}

	public int BatteryRemaining
	{
		get
		{
			return _batteryRemaining;
		}
	}

	public float RollAngle
	{
		get
		{
			return (float)_rollAngle / 10f;
		}
	}

	public float PitchAngle
	{
		get
		{
			return (float)_pitchAngle / 10f;
		}
	}

	public float MotionSpeed
	{
		get
		{
			return _motionSpeed;
		}
	}

	public bool Balance
	{
		get
		{
			return _balance;
		}
	}

	public bool AutoRecovery
	{
		get
		{
			return _autoRecovery;
		}
	}

	public bool Torque
	{
		get
		{
			return _torque;
		}
	}

	public bool Connected
	{
		get
		{
			return _connected;
		}
	}
	
	public void Connect()
	{
		if(commObject == null)
			return;
		
		commObject.Open();
	}
	
	private void ErrorDisconnect()
	{
		bool state = _connected;
		_connected = false;

		commObject.Close();

		if(state == false)
		{
			Debug.Log("Failed to open CommObject!");
			OnConnectionFailed.Invoke();
		}
		else
		{
			Debug.Log("Lost connection!");
			OnDisconnected.Invoke();
		}
	}
	
	public void Disconnect()
	{
		if(commObject == null)
			return;
		
		_connected = false;
		commObject.Close();		
		OnDisconnected.Invoke();
	}
	
	private void TimeoutReset()
	{
		_time = 0;
	}

	private void CommOpenEventHandler(object sender, EventArgs e)
	{
		_connected = true;
		_processRx = false;
		_rxDataBytes.Clear();
		TimeoutReset();
	}
	
	private void CommOpenFailEventHandler(object sender, EventArgs e)
	{
		ErrorDisconnect();
	}
	
	private void CommErrorCloseEventHandler(object sender, EventArgs e)
	{
		ErrorDisconnect();
	}
}
