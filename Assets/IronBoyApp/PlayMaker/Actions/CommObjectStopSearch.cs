using UnityEngine;
using System.Collections;
using SmartMaker;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("SmartMaker")]
    [Tooltip("CommObject.StopSearch()")]
    public class CommObjectStopSearch : FsmStateAction
    {
        [RequiredField]
        public CommObject commObject;

        public override void Reset()
        {
            commObject = null;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (commObject != null)
                commObject.StopSearch();

            Finish();
        }
    }
}
