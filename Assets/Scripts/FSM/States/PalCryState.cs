using UnityEngine;

namespace AI.FSM {
    public class PalCryState : FSMState {
        protected override void init() {
            StateID = FSMStateID.PalCry;
        }
        public override void OnStateEnter(FSMBase fsm) {
            fsm.animator.SetTrigger("cry");

            PalManager manager = fsm.GetComponent<PalManager>();
            if(manager.tip!=null)
                manager.tip.GetComponent<Tips>().OnShow();
            manager.PlayCryClip();
        }
        public override void OnStateExit(FSMBase fsm) {
            fsm.animator.SetTrigger("laugh");

            GameManagerFSM.Instance.Combo++;
            GameManagerFSM.Instance.Score += (int)(GameManagerFSM.Instance.basicScore * GameManagerFSM.Instance.comboMultiplier);
            PalManager manager = fsm.GetComponent<PalManager>();
            manager.tip.GetComponent<Tips>().OnHide();
            manager.PlayInteractClip();
        }
    }

}