using UnityEngine;

namespace AI.FSM {
    public class PalCryState : FSMState {
        protected override void init() {
            StateID = FSMStateID.PalCry;
        }
        public override void OnStateEnter(FSMBase fsm) {
            fsm.GetComponent<PalManager>().tip.GetComponent<Tips>().OnShow();
        }
        public override void OnStateExit(FSMBase fsm) {
            GameManagerFSM.Instance.combo++;
            GameManagerFSM.Instance.score += (int)(GameManagerFSM.Instance.basicScore * GameManagerFSM.Instance.comboMultiplier);
            fsm.GetComponent<PalManager>().tip.GetComponent<Tips>().OnHide();
        }
    }

}