using UnityEngine;

namespace AI.FSM {
    public class GameRunningState : FSMState {
        protected override void init() {
            StateID = FSMStateID.GameRunning;
        }

        public override void OnStateEnter(FSMBase fsm) {
            GameManagerFSM gameManagerFSM = fsm as GameManagerFSM;
            if (gameManagerFSM == null) {
                Debug.LogError("GameRunningState.OnStateEnter: gameManagerFSM is null");
                return;
            }

            gameManagerFSM.restTime = gameManagerFSM.GameTime;
            gameManagerFSM.score = 0;
            gameManagerFSM.combo = 0;
        }

        public override void OnStateStay(FSMBase fsm) {
            GameManagerFSM gameManagerFSM = fsm as GameManagerFSM;

            gameManagerFSM.restTime -= Time.deltaTime;
        }

        public override void OnStateExit(FSMBase fsm) {
            // todo: show game end UI
        }
    }
}