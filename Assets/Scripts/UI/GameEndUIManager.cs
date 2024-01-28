using UnityEngine;
using UnityEngine.UI;

public class GameEndUIManager : MonoBehaviour {
    public Text scoreText;

    public void Start() {
        scoreText.text = GameManagerFSM.Instance.GetFinalScore().ToString();
    }

    public void Restart() {
        GameManagerFSM.Instance.SetTrigger(AI.FSM.FSMTriggerID.GameStart);
        this.gameObject.SetActive(false);
    }

    public void ExitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
