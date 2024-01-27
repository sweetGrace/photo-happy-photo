using UnityEngine;

namespace AI.FSM {
    public class PalCryState : FSMState {
        protected override void init() {
            StateID = FSMStateID.PalCry;
        }
    }

}