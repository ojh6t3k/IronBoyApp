using UnityEngine;
using System;


namespace SmartMaker
{
    [AddComponentMenu("SmartMaker/Arduino/Communication/CommBluetooth")]
    public class CommBluetooth : CommObject
    {
        public float searchTimeout = 5f;

        private float _searchTimeout = 0f;

#if UNITY_ANDROID
        private AndroidJavaObject _android = null;
#endif
        void Awake()
        {
#if UNITY_ANDROID
            try
            {
                AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass pluginClass = new AndroidJavaClass("com.smartmaker.android.CommBluetooth");
                _android = pluginClass.CallStatic<AndroidJavaObject>("GetInstance");
                if (_android.Call<bool>("Initialize", activityContext, gameObject.name) == true)
                {
                    _android.Call("SetUnityMethodOpenSuccess", "AndroidMessageOpenSuccess");
                    _android.Call("SetUnityMethodOpenFailed", "AndroidMessageOpenFailed");
                    _android.Call("SetUnityMethodErrorClose", "AndroidMessageErrorClose");
                    _android.Call("SetUnityMethodFoundDevice", "AndroidMessageFoundDevice");
                }
                else
                    _android = null;
            }
            catch (Exception)
            {
                _android = null;
            }

            if (_android == null)
                Debug.Log("Android Bluetooth Failed!");
            else
                platformSupport = true;
#endif
        }

        void Start()
        {

        }

        void Update()
        {
            if (_searchTimeout > 0f)
            {
                _searchTimeout -= Time.deltaTime;
                if (_searchTimeout <= 0f)
                {
#if UNITY_ANDROID
                    if (_android != null)
                        _android.Call("StopSearch");
#endif
                    OnStopSearch.Invoke();
                }
            }
        }

        #region Override
        public override void Open()
        {
            if (IsOpen)
                return;

#if UNITY_ANDROID
            if (_android != null)
            {
                _android.Call("StopSearch");
                _android.Call("Open", device.address);
            }
#endif
        }

        public override void Close()
        {
            if (!IsOpen)
                return;

            ErrorClose();
            OnClose.Invoke();
        }

        protected override void ErrorClose()
        {
#if UNITY_ANDROID
            if (_android != null)
                _android.Call("Close");
#endif
        }

        public override bool IsOpen
        {
            get
            {
#if UNITY_ANDROID
                if (_android != null)
                    return _android.Call<bool>("IsOpen");
#endif
                return false;
            }
        }

        public override void StartSearch()
        {
            foundDevices.Clear();
            OnStartSearch.Invoke();

#if UNITY_ANDROID
            if (_android != null)
            {
                string[] devInfos = _android.Call<string[]>("GetBondedDevices");
                for (int i = 0; i < devInfos.Length; i++)
                {
                    string[] tokens = devInfos[i].Split(new char[] { ',' });
                    CommDevice foundDevice = new CommDevice();
                    foundDevice.name = tokens[0];
                    foundDevice.address = tokens[1];
                    foundDevices.Add(foundDevice);
                }
                if (devInfos.Length > 0)
                    OnFoundDevice.Invoke();

                _android.Call("StartSearch");
                _searchTimeout = searchTimeout;
            }
#endif
        }

        public override void StopSearch()
        {
            if (_searchTimeout <= 0f)
                return;

#if UNITY_ANDROID
            if (_android != null)
                _android.Call("StopSearch");
#endif
            _searchTimeout = 0f;
            OnStopSearch.Invoke();
        }

        public override void Write(byte[] data)
        {
            if (data == null)
                return;
            if (data.Length == 0)
                return;

#if UNITY_ANDROID
            if (_android != null)
                _android.Call("Write", data);
#endif
        }

        public override byte[] Read()
        {
#if UNITY_ANDROID
            if (_android != null)
            {
                if (_android.Call<int>("Available") > 0)
                    return _android.Call<byte[]>("Read");
            }
#endif
            return null;
        }
        #endregion

#if UNITY_ANDROID
        private void AndroidMessageOpenSuccess(string message)
        {
            Debug.Log(message);
            OnOpen.Invoke();
        }

        private void AndroidMessageOpenFailed(string message)
        {
            Debug.Log(message);
            OnOpenFailed.Invoke();
        }

        private void AndroidMessageErrorClose(string message)
        {
            Debug.Log(message);
            ErrorClose();
            OnErrorClosed.Invoke();
        }

        private void AndroidMessageFoundDevice(string message)
        {
            Debug.Log(message);

            string[] tokens = message.Split(new char[] { ',' });
            CommDevice foundDevice = new CommDevice();
            foundDevice.name = tokens[0];
            foundDevice.address = tokens[1];

            for (int i = 0; i < foundDevices.Count; i++)
            {
                if (foundDevices[i].Equals(foundDevice))
                    return;
            }

            foundDevices.Add(foundDevice);
            OnFoundDevice.Invoke();
        }
#endif

    }
}
