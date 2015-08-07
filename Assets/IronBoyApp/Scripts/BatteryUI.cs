using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class BatteryUI : MonoBehaviour
{
	public Image uiImage;
	public Text uiText;
	public Sprite image100;
	public Sprite image80;
	public Sprite image60;
	public Sprite image40;
	public Sprite image20;

	private int _value;

	// Use this for initialization
	void Start ()
	{
		Value = 100;	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private Sprite GetSprite()
	{
		if(_value > 80)
			return image100;
		else if(_value > 60)
			return image80;
		else if(_value > 40)
			return image60;
		else if(_value > 20)
			return image40;
		else
			return image20;
	}

	public int Value
	{
		set
		{
			_value = Mathf.Clamp(value, 0, 100);
			if(uiImage != null)
				uiImage.sprite = GetSprite();
			if(uiText != null)
				uiText.text = string.Format("{0:d}%", _value);
		}
		get
		{
			return _value;
		}
	}
}
