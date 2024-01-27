using UnityEngine;

namespace AI.FSM {
    public class PalCryState : FSMState {
        protected override void init() {
            StateID = FSMStateID.PalCry;
        }
        public override void OnStateEnter(FSMBase fsm) {
            fsm.transform.position = new Vector3(
                fsm.transform.position.x,
                fsm.transform.position.y + 2,
                fsm.transform.position.z
            );
        }
        public override void OnStateExit(FSMBase fsm) {
            fsm.transform.position = new Vector3(
                fsm.transform.position.x,
                fsm.transform.position.y - 2,
                fsm.transform.position.z
            );
        }
    }

}