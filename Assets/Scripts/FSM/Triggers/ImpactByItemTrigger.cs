using UnityEngine;

namespace AI.FSM {

    public class ImpactByItemTrigger : FSMTrigger {
        protected override void init() {
            TriggerID = FSMTriggerID.ImpactByItem;
        }

        public override bool HandleTrigger(FSMBase fsm) {
            return false;  // only set manually
        }
    }

}