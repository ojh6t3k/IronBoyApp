using UnityEngine;
using System.Collections.Generic;
using SmartMaker;
using UnityEngine.UI;


public class CommDeviceListUI : MonoBehaviour
{
	public CommObject commObject;
	public ListItem uiDeviceItem;
    public ListView uiDeviceList;
    public Button uiSearch;

    private bool _preventEvent = false;
    private List<CommDevice> _lastFoundDevices = new List<CommDevice>();

    void Awake()
    {
        if (commObject != null)
        {
            commObject.OnStartSearch.AddListener(OnStartSearch);
            commObject.OnFoundDevice.AddListener(OnFoundDevice);
            commObject.OnStopSearch.AddListener(OnStopSearch);
        }

        if(uiDeviceList != null)
        {
            uiDeviceList.OnChangedSelection.AddListener(OnChangedSelection);
        }

        if (uiSearch != null)
            uiSearch.onClick.AddListener(OnSearchClick);
    }

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void OnChangedSelection()
	{
        if (_preventEvent)
            return;

        ListItem selected = uiDeviceList.selectedItem;
        if (selected != null)
            commObject.device = new CommDevice((CommDevice)selected.data);
    }

    private void OnSearchClick()
    {
        if (commObject != null)
        {
            commObject.StartSearch();
            uiSearch.interactable = false;
        }
    }

    private void OnStartSearch()
    {
        if (uiDeviceList != null)
            uiDeviceList.ClearItem();

        _lastFoundDevices.Clear();
    }

    private void OnFoundDevice()
    {
        if(_lastFoundDevices.Count < commObject.foundDevices.Count)
        {
            for(int i=_lastFoundDevices.Count; i<commObject.foundDevices.Count; i++)
            {
                _lastFoundDevices.Add(commObject.foundDevices[i]);
                ListItem item = GameObject.Instantiate(uiDeviceItem);
                item.textList[0].text = commObject.foundDevices[i].name;
                item.data = commObject.foundDevices[i];
                uiDeviceList.AddItem(item);
            }
        }
        
        _preventEvent = true;
        for (int i = 0; i < commObject.foundDevices.Count; i++)
        {
            if (commObject.device.Equals(commObject.foundDevices[i]))
            {
                uiDeviceList.selectedIndex = i;
                break;
            }
        }
        _preventEvent = false;
    }

    private void OnStopSearch()
    {
        if(uiSearch != null)
            uiSearch.interactable = true;
    }
}
