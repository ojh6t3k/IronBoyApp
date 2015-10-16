using UnityEngine;
using System.Collections;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("SmartMaker")]
	[Tooltip("ListView.AddItem()")]
	public class ListViewAddItem : FsmStateAction
	{
		[RequiredField]
		public ListView listView;
		public ListItem listItem;
		public Sprite sprite;
		public FsmString[] text;
		public FsmObject data;

		public override void Reset()
		{
			listView = null;
			listItem = null;
			sprite = null;
            text = new FsmString[0];
			data = new FsmObject { UseVariable = true };
		}
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			if(listView != null && listItem != null)
			{
                ListItem item = GameObject.Instantiate(listItem);
                item.image.sprite = sprite;
                for (int i=0; i< text.Length; i++)
                {
                    if(!text[i].IsNone)
                        item.textList[i].text = text[i].Value;
                }
                if (!data.IsNone)
                    item.data = data.Value;

                listView.AddItem(listItem);
			}

			Finish();
		}
	}
}
