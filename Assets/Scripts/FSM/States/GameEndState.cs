using UnityEngine;

namespace AI.FSM {
    public class GameEndState : FSMState {
        protected override void init() {
            StateID = FSMStateID.GameEnd;
        }

        public override void OnStateEnter(FSMBase fsm) {
            Time.timeScale = 0;
        }
    }
}