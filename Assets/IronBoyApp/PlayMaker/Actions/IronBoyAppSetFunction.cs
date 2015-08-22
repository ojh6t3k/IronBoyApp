using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("IronBoy")]
	[Tooltip("IronBoyApp.SetFunction()")]
	public class IronBoyAppSetFunction : FsmStateAction
	{
		[RequiredField]
		public IronBoyApp ironBoyApp;
		[RequiredField]
		public FsmInt robotID;
		[RequiredField]
		public FsmFloat motionSpeed;
		[RequiredField]
		public FsmBool balance;
		[RequiredField]
		public FsmBool autoRecovery;
		[RequiredField]
		public FsmBool torque;

		public override void Reset()
		{
			ironBoyApp = null;
			robotID = new FsmInt { UseVariable = true };
			motionSpeed = new FsmFloat { UseVariable = true };
			balance = new FsmBool { UseVariable = true };
			autoRecovery = new FsmBool { UseVariable = true };
			torque = new FsmBool { UseVariable = true };
		}
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			if(ironBoyApp != null)
			{
				int id = 0;
				if(!robotID.IsNone)
					id = robotID.Value;
				
				float speed = ironBoyApp.MotionSpeed;
				if(!motionSpeed.IsNone)
					speed = motionSpeed.Value;
				
				bool bal = ironBoyApp.Balance;
				if(!balance.IsNone)
					bal = balance.Value;
				
				bool auto = ironBoyApp.AutoRecovery;
				if(!autoRecovery.IsNone)
					auto = autoRecovery.Value;
				
				bool tq = ironBoyApp.Torque;
				if(!torque.IsNone)
					tq = torque.Value;
				
				ironBoyApp.SetFunction(id, speed, bal, auto, tq);
			}
			
			Finish();
		}
	}
}
