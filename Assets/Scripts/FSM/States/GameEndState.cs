using UnityEngine;

namespace AI.FSM {
    public class GameEndState : FSMState {
        protected override void init() {
            StateID = FSMStateID.GameEnd;
        }
    }
}