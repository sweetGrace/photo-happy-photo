using UnityEngine;

namespace AI.FSM {
    public abstract class UpdateByInterval : FSMTrigger {
        private float lastHandleTime = 0f;
        public override bool HandleTrigger(FSMBase fsm) {
            if (Time.time - lastHandleTime < Config.Instance.palStateUpdateInterval) {
                return false;
            }
            lastHandleTime = Time.time;
            /*if (GameManagerFSM.Instance.CurrentState != FSMStateID.GameRunning)
                return false;*/
            return Update(fsm);
        }
        protected abstract bool Update(FSMBase fsm);
    }

}