using System.Linq;
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
            gameManagerFSM.Score = 0;
            gameManagerFSM.Combo = 0;
            gameManagerFSM.cameraed = false;
            PlayerController.Instance.transform.position = gameManagerFSM.defaultPlayerPosition.position;
            GameObject.FindGameObjectsWithTag("Pal").ToList().ForEach(pal => pal.GetComponent<PalFSM>().loadDefaultState());
            for (int i = 0; i < gameManagerFSM.items.Length; i++) {
                gameManagerFSM.items[i].transform.position = gameManagerFSM.itemPositions[i].position;
                gameManagerFSM.items[i].transform.rotation = gameManagerFSM.itemPositions[i].rotation;
            }

            Time.timeScale = 1;
        }

        public override void OnStateStay(FSMBase fsm) {
            GameManagerFSM gameManagerFSM = fsm as GameManagerFSM;

            gameManagerFSM.restTime -= Time.deltaTime;
        }

        public override void OnStateExit(FSMBase fsm) {
            GameManagerFSM gameManagerFSM = fsm as GameManagerFSM;

            gameManagerFSM.gameEndUI.SetActive(true);
        }
    }
}