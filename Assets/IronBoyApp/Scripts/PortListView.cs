using UnityEngine;
using System.Collections;
using SmartMaker;


[RequireComponent(typeof(ListView))]
public class PortListView : MonoBehaviour
{
	public CommObject commObject;
	public ListItem listItem;

	private CommBluetooth _commBluetooth;
	private ListView _listView;

	// Use this for initialization
	void Start ()
	{
		if(commObject != null)
		{
			_commBluetooth = (CommBluetooth)commObject;
			if(_commBluetooth == null)
			{
			}
		}

		_listView = GetComponent<ListView>();
		if(_listView != null)
			_listView.OnChangedSelection.AddListener(OnChangedSelection);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Refresh()
	{
		if(_listView != null)
			_listView.ClearItem();

		if(_commBluetooth != null)
		{
			_commBluetooth.DeviceSearch();
			for(int i=0; i<_commBluetooth.devNames.Count; i++)
				_listView.AddItem(listItem, null, _commBluetooth.devNames[i], null);
		}
	}

	private void OnChangedSelection()
	{
		ListItem selectedItem = _listView.selectedItem;
		if(_commBluetooth != null)
		{
			_commBluetooth.devName = selectedItem.text.text;
		}
	}
}
