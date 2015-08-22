using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
	public ListView uiList;
	public InputField uiInput;

	public int buttonU = 0;
	public int buttonUR = 0;
	public int buttonR = 0;
	public int buttonDR = 0;
	public int buttonD = 0;
	public int buttonDL = 0;
	public int buttonL = 0;
	public int buttonUL = 0;
	public int buttonC = 0;
	public int button1 = 0;
	public int button2 = 0;
	public int button3 = 0;
	public int button4 = 0;
	public int button5 = 0;
	public int button6 = 0;
	public int button7 = 0;
	public int button8 = 0;
	public int button9 = 0;
	public int button10 = 0;

	private string _buttonName;

	// Use this for initialization
	void Start ()
	{
		if(uiList != null)
			uiList.OnChangedSelection.AddListener(OnChangedSelection);

		if(uiInput != null)
		{
			uiInput.interactable = false;
			uiInput.onEndEdit.AddListener(OnEndEdit);
		}

		Load();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void OnChangedSelection()
	{
		ListItem selectedItem = uiList.selectedItem;
		if(selectedItem == null)
		{
			_buttonName = "";
			if(uiInput != null)
			{
				uiInput.interactable = false;
				uiInput.text = "";
			}
		}
		else
		{
			_buttonName = selectedItem.gameObject.name;
			if(uiInput != null)
			{
				uiInput.interactable = true;
				uiInput.text = selectedItem.text.text;
			}
		}
	}

	private void OnEndEdit(string value)
	{
		SetButton(_buttonName, int.Parse(value));
	}

	public void Load()
	{
		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonU"))
			buttonU = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonU");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonUR"))
			buttonUR = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonUR");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonR"))
			buttonR = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonR");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonDR"))
			buttonDR = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonDR");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonD"))
			buttonD = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonD");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonDL"))
			buttonDL = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonDL");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonL"))
			buttonL = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonL");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonUL"))
			buttonUL = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonUL");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.ButtonC"))
			buttonC = PlayerPrefs.GetInt("IronBoyApp.Config.ButtonC");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button1"))
			button1 = PlayerPrefs.GetInt("IronBoyApp.Config.Button1");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button2"))
			button2 = PlayerPrefs.GetInt("IronBoyApp.Config.Button2");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button3"))
			button3 = PlayerPrefs.GetInt("IronBoyApp.Config.Button3");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button4"))
			button4 = PlayerPrefs.GetInt("IronBoyApp.Config.Button4");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button5"))
			button5 = PlayerPrefs.GetInt("IronBoyApp.Config.Button5");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button6"))
			button6 = PlayerPrefs.GetInt("IronBoyApp.Config.Button6");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button7"))
			button7 = PlayerPrefs.GetInt("IronBoyApp.Config.Button7");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button8"))
			button8 = PlayerPrefs.GetInt("IronBoyApp.Config.Button8");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button9"))
			button9 = PlayerPrefs.GetInt("IronBoyApp.Config.Button9");

		if(PlayerPrefs.HasKey("IronBoyApp.Config.Button10"))
			button10 = PlayerPrefs.GetInt("IronBoyApp.Config.Button10");

		if(uiList != null)
		{
			uiList.selectedItem = null;
			SetButton("ButtonU", buttonU);
			SetButton("ButtonUR", buttonUR);
			SetButton("ButtonR", buttonR);
			SetButton("ButtonDR", buttonDR);
			SetButton("ButtonD", buttonD);
			SetButton("ButtonDL", buttonDL);
			SetButton("ButtonL", buttonL);
			SetButton("ButtonUL", buttonUL);
			SetButton("ButtonC", buttonC);
			SetButton("Button1", button1);
			SetButton("Button2", button2);
			SetButton("Button3", button3);
			SetButton("Button4", button4);
			SetButton("Button5", button5);
			SetButton("Button6", button6);
			SetButton("Button7", button7);
			SetButton("Button8", button8);
			SetButton("Button9", button9);
			SetButton("Button10", button10);
		}

		if(uiInput != null)
		{
			uiInput.interactable = false;
			uiInput.text = "";
		}
	}

	public void Save()
	{
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonU", buttonU);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonUR", buttonUR);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonR", buttonR);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonDR", buttonDR);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonD", buttonD);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonDL", buttonDL);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonL", buttonL);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonUL", buttonUL);
		PlayerPrefs.SetInt("IronBoyApp.Config.ButtonC", buttonC);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button1", button1);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button2", button2);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button3", button3);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button4", button4);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button5", button5);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button6", button6);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button7", button7);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button8", button8);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button9", button9);
		PlayerPrefs.SetInt("IronBoyApp.Config.Button10", button10);
	}

	public void GetButton(string buttonName, int buttonValue)
	{
	}

	public void SetButton(string buttonName, int buttonValue)
	{
		ListItem item = null;
		foreach(Transform child in uiList.itemPanel.transform)
		{
			if(child.gameObject.name.Equals(buttonName))
			{
				item = child.GetComponent<ListItem>();
				break;
			}
		}

		if(item == null)
			return;

		item.text.text = buttonValue.ToString();
		if(buttonName.Equals("ButtonU"))
			buttonU = buttonValue;
		else if(buttonName.Equals("ButtonUR"))
			buttonUR = buttonValue;
		else if(buttonName.Equals("ButtonR"))
			buttonR = buttonValue;
		else if(buttonName.Equals("ButtonUR"))
			buttonUR = buttonValue;
		else if(buttonName.Equals("ButtonD"))
			buttonD = buttonValue;
		else if(buttonName.Equals("ButtonDL"))
			buttonDL = buttonValue;
		else if(buttonName.Equals("ButtonL"))
			buttonL = buttonValue;
		else if(buttonName.Equals("ButtonUL"))
			buttonUL = buttonValue;
		else if(buttonName.Equals("Button1"))
			button1 = buttonValue;
		else if(buttonName.Equals("Button2"))
			button2 = buttonValue;
		else if(buttonName.Equals("Button3"))
			button3 = buttonValue;
		else if(buttonName.Equals("Button4"))
			button4 = buttonValue;
		else if(buttonName.Equals("Button5"))
			button5 = buttonValue;
		else if(buttonName.Equals("Button6"))
			button6 = buttonValue;
		else if(buttonName.Equals("Button7"))
			button7 = buttonValue;
		else if(buttonName.Equals("Button8"))
			button8 = buttonValue;
		else if(buttonName.Equals("Button9"))
			button9 = buttonValue;
		else if(buttonName.Equals("Button10"))
			button10 = buttonValue;
	}
}
