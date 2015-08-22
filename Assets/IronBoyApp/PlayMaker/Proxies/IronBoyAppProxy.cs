using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using HutongGames.PlayMaker;

public class IronBoyAppProxy : MonoBehaviour
{
	public readonly string builtInOnConnected = "IRONBOY APP / ON CONNECTED";
	public readonly string builtInOnConnectionFailed = "IRONBOY APP / ON CONNECTION FAILED";
	public readonly string builtInOnDisconnected = "IRONBOY APP / ON DISCONNECTED";
	
	public string eventOnConnected = "IRONBOY APP / ON CONNECTED";
	public string eventOnConnectionFailed = "IRONBOY APP / ON CONNECTION FAILED";
	public string eventOnDisconnected = "IRONBOY APP / ON DISCONNECTED";

	private IronBoyApp _ironBoy;
	private PlayMakerFSM _fsm;
	private FsmEventTarget _fsmEventTarget;


	// Use this for initialization
	void Start ()
	{
		_fsm = FindObjectOfType<PlayMakerFSM>();
		if(_fsm == null)
			_fsm = gameObject.AddComponent<PlayMakerFSM>();
		
		_ironBoy = GetComponent<IronBoyApp>();
		if(_ironBoy != null)
		{
			_ironBoy.OnConnected.AddListener(OnConnected);
			_ironBoy.OnConnectionFailed.AddListener(OnConnectionFailed);
			_ironBoy.OnDisconnected.AddListener(OnDisconnected);
		}
		
		_fsmEventTarget = new FsmEventTarget();
		_fsmEventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
		_fsmEventTarget.excludeSelf = false;	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void OnConnected()
	{
		_fsm.Fsm.Event(_fsmEventTarget, eventOnConnected);
	}

	private void OnConnectionFailed()
	{
		_fsm.Fsm.Event(_fsmEventTarget, eventOnConnectionFailed);
	}

	private void OnDisconnected()
	{
		_fsm.Fsm.Event(_fsmEventTarget, eventOnDisconnected);
	}
}
