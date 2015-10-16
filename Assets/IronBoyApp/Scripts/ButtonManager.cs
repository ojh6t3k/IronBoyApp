using UnityEngine;
using System;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
	public ListView uiList;
	public InputField uiInput;

	public int buttonU = -1;
	public int buttonUR = -1;
	public int buttonR = -1;
	public int buttonDR = -1;
	public int buttonD = -1;
	public int buttonDL = -1;
	public int buttonL = -1;
	public int buttonUL = -1;
	public int buttonC = -1;
	public int button1 = -1;
	public int button2 = -1;
	public int button3 = -1;
	public int button4 = -1;
	public int button5 = -1;
	public int button6 = -1;
	public int button7 = -1;
	public int button8 = -1;
	public int button9 = -1;
	public int button10 = -1;

    private const string _keyButtonU = "IronBoyApp.Config.ButtonU";
    private const string _keyButtonUR = "IronBoyApp.Config.ButtonUR";
    private const string _keyButtonR = "IronBoyApp.Config.ButtonR";
    private const string _keyButtonDR = "IronBoyApp.Config.ButtonDR";
    private const string _keyButtonD = "IronBoyApp.Config.ButtonD";
    private const string _keyButtonDL = "IronBoyApp.Config.ButtonDL";
    private const string _keyButtonL = "IronBoyApp.Config.ButtonL";
    private const string _keyButtonUL = "IronBoyApp.Config.ButtonUL";
    private const string _keyButtonC = "IronBoyApp.Config.ButtonC";
    private const string _keyButton1 = "IronBoyApp.Config.Button1";
    private const string _keyButton2 = "IronBoyApp.Config.Button2";
    private const string _keyButton3 = "IronBoyApp.Config.Button3";
    private const string _keyButton4 = "IronBoyApp.Config.Button4";
    private const string _keyButton5 = "IronBoyApp.Config.Button5";
    private const string _keyButton6 = "IronBoyApp.Config.Button6";
    private const string _keyButton7 = "IronBoyApp.Config.Button7";
    private const string _keyButton8 = "IronBoyApp.Config.Button8";
    private const string _keyButton9 = "IronBoyApp.Config.Button9";
    private const string _keyButton10 = "IronBoyApp.Config.Button10";

    private string _selectedButton = "";

	void Awake()
	{
		Load();
	}

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
            _selectedButton = "";
			if(uiInput != null)
			{
				uiInput.interactable = false;
				uiInput.text = "";
			}
		}
		else
		{
            _selectedButton = selectedItem.gameObject.name;
			if(uiInput != null)
			{
				uiInput.interactable = true;
				uiInput.text = selectedItem.textList[0].text;
			}
		}
	}

	private void OnEndEdit(string value)
	{
        try
        {
            SetButton(_selectedButton, int.Parse(value));
        }
        catch(Exception)
        {

        }		
	}

	public void Load()
	{
        if (PlayerPrefs.HasKey(_keyButtonU))
            buttonU = PlayerPrefs.GetInt(_keyButtonU);

        if (PlayerPrefs.HasKey(_keyButtonUR))
            buttonUR = PlayerPrefs.GetInt(_keyButtonUR);

		if(PlayerPrefs.HasKey(_keyButtonR))
			buttonR = PlayerPrefs.GetInt(_keyButtonR);

		if(PlayerPrefs.HasKey(_keyButtonDR))
			buttonDR = PlayerPrefs.GetInt(_keyButtonDR);

		if(PlayerPrefs.HasKey(_keyButtonD))
			buttonD = PlayerPrefs.GetInt(_keyButtonD);

		if(PlayerPrefs.HasKey(_keyButtonDL))
			buttonDL = PlayerPrefs.GetInt(_keyButtonDL);

		if(PlayerPrefs.HasKey(_keyButtonL))
			buttonL = PlayerPrefs.GetInt(_keyButtonL);

		if(PlayerPrefs.HasKey(_keyButtonUL))
			buttonUL = PlayerPrefs.GetInt(_keyButtonUL);

		if(PlayerPrefs.HasKey(_keyButtonC))
			buttonC = PlayerPrefs.GetInt(_keyButtonC);

		if(PlayerPrefs.HasKey(_keyButton1))
			button1 = PlayerPrefs.GetInt(_keyButton1);

		if(PlayerPrefs.HasKey(_keyButton2))
			button2 = PlayerPrefs.GetInt(_keyButton2);

		if(PlayerPrefs.HasKey(_keyButton3))
			button3 = PlayerPrefs.GetInt(_keyButton3);

		if(PlayerPrefs.HasKey(_keyButton4))
			button4 = PlayerPrefs.GetInt(_keyButton4);

		if(PlayerPrefs.HasKey(_keyButton5))
			button5 = PlayerPrefs.GetInt(_keyButton5);

		if(PlayerPrefs.HasKey(_keyButton6))
			button6 = PlayerPrefs.GetInt(_keyButton6);

		if(PlayerPrefs.HasKey(_keyButton7))
			button7 = PlayerPrefs.GetInt(_keyButton7);

		if(PlayerPrefs.HasKey(_keyButton8))
			button8 = PlayerPrefs.GetInt(_keyButton8);

		if(PlayerPrefs.HasKey(_keyButton9))
			button9 = PlayerPrefs.GetInt(_keyButton9);

		if(PlayerPrefs.HasKey(_keyButton10))
			button10 = PlayerPrefs.GetInt(_keyButton10);

        UpdateButtonUI();

        if (uiInput != null)
		{
			uiInput.interactable = false;
			uiInput.text = "";
		}
	}

	public void Save()
	{
        PlayerPrefs.SetInt(_keyButtonU, buttonU);
        PlayerPrefs.SetInt(_keyButtonUR, buttonUR);
        PlayerPrefs.SetInt(_keyButtonR, buttonR);
        PlayerPrefs.SetInt(_keyButtonDR, buttonDR);
        PlayerPrefs.SetInt(_keyButtonD, buttonD);
        PlayerPrefs.SetInt(_keyButtonDL, buttonDL);
        PlayerPrefs.SetInt(_keyButtonL, buttonL);
        PlayerPrefs.SetInt(_keyButtonUL, buttonUL);
        PlayerPrefs.SetInt(_keyButtonC, buttonC);
        PlayerPrefs.SetInt(_keyButton1, button1);
        PlayerPrefs.SetInt(_keyButton2, button2);
        PlayerPrefs.SetInt(_keyButton3, button3);
        PlayerPrefs.SetInt(_keyButton4, button4);
        PlayerPrefs.SetInt(_keyButton5, button5);
        PlayerPrefs.SetInt(_keyButton6, button6);
        PlayerPrefs.SetInt(_keyButton7, button7);
        PlayerPrefs.SetInt(_keyButton8, button8);
        PlayerPrefs.SetInt(_keyButton9, button9);
        PlayerPrefs.SetInt(_keyButton10, button10);
	}

    public void ButtonReset()
    {
        SetButton(_selectedButton, -1);

        if (uiInput != null)
        {
            if(uiInput.interactable)
                uiInput.text = "None";
        }
    }

    public void ButtonResetAll()
    {
        buttonU = -1;
        buttonUR = -1;
        buttonR = -1;
        buttonDR = -1;
        buttonD = -1;
        buttonDL = -1;
        buttonL = -1;
        buttonUL = -1;
        buttonC = -1;
        button1 = -1;
        button2 = -1;
        button3 = -1;
        button4 = -1;
        button5 = -1;
        button6 = -1;
        button7 = -1;
        button8 = -1;
        button9 = -1;
        button10 = -1;

        UpdateButtonUI();
        OnChangedSelection();
    }

    private void UpdateButtonUI()
    {
        if (uiList != null)
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

        if (buttonValue >= 0)
            item.textList[0].text = buttonValue.ToString();
        else
            item.textList[0].text = "None";

        if (buttonName.Equals("ButtonU"))
			buttonU = buttonValue;
		else if(buttonName.Equals("ButtonUR"))
			buttonUR = buttonValue;
		else if(buttonName.Equals("ButtonR"))
			buttonR = buttonValue;
		else if(buttonName.Equals("ButtonDR"))
			buttonDR = buttonValue;
		else if(buttonName.Equals("ButtonD"))
			buttonD = buttonValue;
		else if(buttonName.Equals("ButtonDL"))
			buttonDL = buttonValue;
		else if(buttonName.Equals("ButtonL"))
			buttonL = buttonValue;
		else if(buttonName.Equals("ButtonUL"))
			buttonUL = buttonValue;
		else if(buttonName.Equals("ButtonC"))
			buttonC = buttonValue;
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
