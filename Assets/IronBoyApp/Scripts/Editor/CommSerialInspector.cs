using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CommSerial))]
public class CommSerialInspector : Editor
{
	bool foldout = true;

	SerializedProperty portNames;
	SerializedProperty portName;
	SerializedProperty baudrate;
	SerializedProperty debugDisplay;
	SerializedProperty uiText;
	SerializedProperty uiPanel;
	SerializedProperty uiItem;

	void OnEnable()
	{
		portNames = serializedObject.FindProperty("portNames");
		portName = serializedObject.FindProperty("portName");
		baudrate = serializedObject.FindProperty("baudrate");
		debugDisplay = serializedObject.FindProperty("debugDisplay");
		uiText = serializedObject.FindProperty("uiText");
		uiPanel = serializedObject.FindProperty("uiPanel");
		uiItem = serializedObject.FindProperty("uiItem");
	}
	
	public override void OnInspectorGUI()
	{
#if !UNITY_STANDALONE
		EditorGUILayout.HelpBox("This component only can work on standalone platform(windows, osx, linux..)", MessageType.Error);
#endif
		this.serializedObject.Update();

		CommSerial serial = (CommSerial)target;

		GUI.enabled = !serial.IsOpen;

#if UINTY_STANDALONE_OSX || UNITY_EDITOR_OSX
		EditorGUILayout.PropertyField(portName, new GUIContent("Port Name (/dev/)"));
#else
		EditorGUILayout.PropertyField(portName, new GUIContent("Port Name"));
#endif
		EditorGUILayout.BeginHorizontal();
		int index = -1;
		string[] list = new string[portNames.arraySize];
		for(int i=0; i<list.Length; i++)
		{
			list[i] = portNames.GetArrayElementAtIndex(i).stringValue;
			if(portName.stringValue.Equals(list[i]) == true)
				index = i;
		}
		index = EditorGUILayout.Popup(" ", index, list);
		if(index >= 0)
			portName.stringValue = list[index];
		if(GUILayout.Button("Search", GUILayout.Width(60f)) == true)
			serial.PortSearch();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.PropertyField(baudrate, new GUIContent("Baudrate"));
		EditorGUILayout.PropertyField(debugDisplay, new GUIContent("Debug Display"));

		foldout = EditorGUILayout.Foldout(foldout, "UI objects");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(uiText, new GUIContent("UI Text"));
			EditorGUILayout.PropertyField(uiPanel, new GUIContent("UI Panel"));
			EditorGUILayout.PropertyField(uiItem, new GUIContent("UI Item"));
			EditorGUI.indentLevel--;
		}

		this.serializedObject.ApplyModifiedProperties();
	}
}
