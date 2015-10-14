using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Events;

[CustomEditor(typeof(IronBoyApp))]
public class IronBoyAppInspector : Editor
{
    SerializedProperty commObject;
    SerializedProperty timeoutSec;
	SerializedProperty OnConnected;
	SerializedProperty OnConnectionFailed;
	SerializedProperty OnDisconnected;
    SerializedProperty OnLostConnection;

    void OnEnable()
	{
        commObject = serializedObject.FindProperty("commObject");
        timeoutSec = serializedObject.FindProperty("timeoutSec");
		OnConnected = serializedObject.FindProperty("OnConnected");
		OnConnectionFailed = serializedObject.FindProperty("OnConnectionFailed");
		OnDisconnected = serializedObject.FindProperty("OnDisconnected");
        OnLostConnection = serializedObject.FindProperty("OnLostConnection");
    }
	
	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();
		
		IronBoyApp ironBoy = (IronBoyApp)target;
		
		if(Application.isPlaying == false)
		{
			EditorGUILayout.HelpBox("To connect the board is only possible in Play mode.", MessageType.Info);
		}
		else
		{
			if(ironBoy.commObject != null)
			{
				if(ironBoy.connected == true)
				{
					if(GUILayout.Button("Disconnect") == true)
						ironBoy.Disconnect();
				}
				else
				{
					if(GUILayout.Button("Connect") == true)
						ironBoy.Connect();
				}

				EditorUtility.SetDirty(target);
			}
			else
			{
				EditorGUILayout.HelpBox("CommObject is Null!", MessageType.Error);
			}
		}

        EditorGUILayout.PropertyField(commObject, new GUIContent("CommObject"));
        EditorGUILayout.PropertyField(timeoutSec, new GUIContent("Timeout(sec)"));
		
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnConnected);
		EditorGUILayout.PropertyField(OnConnectionFailed);
		EditorGUILayout.PropertyField(OnDisconnected);
        EditorGUILayout.PropertyField(OnLostConnection);

        this.serializedObject.ApplyModifiedProperties();
	}
}
