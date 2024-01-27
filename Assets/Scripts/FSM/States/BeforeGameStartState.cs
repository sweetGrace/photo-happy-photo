using UnityEngine;

namespace AI.FSM {
    public class BeforeGameStartState : FSMState {
        protected override void init() {
            StateID = FSMStateID.BeforeGameStart;
        }
    }
}