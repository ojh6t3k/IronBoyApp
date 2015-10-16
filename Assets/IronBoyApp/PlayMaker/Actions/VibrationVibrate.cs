using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("SmartMaker")]
    [Tooltip("Vibration.Vibrate()")]
    public class VibrationVibrate : FsmStateAction
    {
        [RequiredField]
        public Vibration vibration;
        public FsmInt milliseconds;

        public override void Reset()
        {
            vibration = null;
            milliseconds = new FsmInt { UseVariable = false, Value = 1000 };
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            if (vibration != null)
            {
                if (!milliseconds.IsNone)
                    vibration.Vibrate(milliseconds.Value);
            }

            Finish();
        }
    }
}
