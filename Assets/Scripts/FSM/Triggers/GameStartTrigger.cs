using UnityEngine;

namespace AI.FSM {
    public class GameStartTrigger : FSMTrigger {
        protected override void init() {
            TriggerID = FSMTriggerID.GameStart;
        }

        public override bool HandleTrigger(FSMBase fsm) {
            return false;
        }
    }
}