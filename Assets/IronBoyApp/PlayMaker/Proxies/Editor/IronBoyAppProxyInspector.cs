using UnityEngine;
using System.Collections;
using UnityEditor;
using HutongGames.PlayMaker;


[CustomEditor(typeof(IronBoyAppProxy))]
public class IronBoyAppProxyInspector : Editor
{
	public override void OnInspectorGUI()
	{
		IronBoyAppProxy proxy = (IronBoyAppProxy)target;
		
		if(proxy.GetComponent<IronBoyApp>() == null)
		{
			EditorGUILayout.HelpBox("There is no IronBoyApp!", MessageType.Error);
		}
		else
		{
			proxy.eventOnConnected = ProxyInspectorUtil.EventField(target, "OnConnected", proxy.eventOnConnected, proxy.builtInOnConnected);
			proxy.eventOnConnectionFailed = ProxyInspectorUtil.EventField(target, "OnConnectionFailed", proxy.eventOnConnectionFailed, proxy.builtInOnConnectionFailed);
			proxy.eventOnDisconnected = ProxyInspectorUtil.EventField(target, "OnDisonnected", proxy.eventOnDisconnected, proxy.builtInOnDisconnected);
            proxy.eventOnLostConnection = ProxyInspectorUtil.EventField(target, "OnLostConnection", proxy.eventOnLostConnection, proxy.builtInOnLostConnection);
        }
	}
}
