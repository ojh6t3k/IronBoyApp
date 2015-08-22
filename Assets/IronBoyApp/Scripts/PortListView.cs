using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ListView))]
public class PortListView : MonoBehaviour
{
	public CommObject commObject;
	public ListItem listItem;

	private CommSerial _commSerial;
	private ListView _listView;

	// Use this for initialization
	void Start ()
	{
		if(commObject != null)
		{
			_commSerial = (CommSerial)commObject;
			if(_commSerial == null)
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

		if(_commSerial != null)
		{
			_commSerial.PortSearch();
			for(int i=0; i<_commSerial.portNames.Count; i++)
				_listView.AddItem(listItem, null, _commSerial.portNames[i], null);
		}
	}

	private void OnChangedSelection()
	{
		ListItem selectedItem = _listView.selectedItem;
		if(_commSerial != null)
		{
			_commSerial.portName = selectedItem.text.text;
		}
	}
}
