using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("IronBoy")]
	[Tooltip("IronBoyApp.RunMotion()")]
	public class IronBoyAppRunMotion : FsmStateAction
	{
		[RequiredField]
		public IronBoyApp ironBoyApp;
		[RequiredField]
		public FsmInt robotID;
		[RequiredField]
		public FsmInt motionIndex;
		[RequiredField]
		public FsmFloat leftVertical;
		[RequiredField]
		public FsmFloat leftHorizontal;
		[RequiredField]
		public FsmFloat rightVertical;
		[RequiredField]
		public FsmFloat rightHorizontal;

		public override void Reset()
		{
			ironBoyApp = null;
			robotID = new FsmInt { UseVariable = true };
			motionIndex = new FsmInt { UseVariable = true };
			leftVertical = new FsmFloat { UseVariable = true };
			leftHorizontal = new FsmFloat { UseVariable = true };
			rightVertical = new FsmFloat { UseVariable = true };
			rightHorizontal = new FsmFloat { UseVariable = true };
		}
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			if(ironBoyApp != null)
			{
				int id = 0;
				if(!robotID.IsNone)
					id = robotID.Value;
				
				int index = 0;
				if(!motionIndex.IsNone)
					index = motionIndex.Value;

				float lV = 0;
				if(!leftVertical.IsNone)
					lV = leftVertical.Value;

				float lH = 0;
				if(!leftHorizontal.IsNone)
					lH = leftHorizontal.Value;

				float rV = 0;
				if(!rightVertical.IsNone)
					rV = rightVertical.Value;

				float rH = 0;
				if(!rightHorizontal.IsNone)
					rH = rightHorizontal.Value;
				
				ironBoyApp.RunMotion(id, index, lV, lH, rV, rH);
			}
			
			Finish();
		}
	}
}
