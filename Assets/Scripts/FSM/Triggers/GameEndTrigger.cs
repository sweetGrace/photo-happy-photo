using UnityEngine;

namespace AI.FSM {
    public class GameEndTrigger : FSMTrigger {
        protected override void init() {
            TriggerID = FSMTriggerID.GameEnd;
        }

        public override bool HandleTrigger(FSMBase fsm) {
            GameManagerFSM gameManagerFSM = fsm as GameManagerFSM;
            if (gameManagerFSM == null) {
                Debug.LogError("GameManagerFSM is null");
                return false;
            }

            return gameManagerFSM.restTime <= 0;
        }
    }
}