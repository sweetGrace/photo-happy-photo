using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartMenuManager : MonoBehaviour {
    [Header("Curtain")]
    public GameObject leftCurtain;
    public GameObject rightCurtain;
    public float curtainMoveDistance;
    public float duration;
    public AudioSource audioSource;
    public GameObject title;
    public GameObject startButton;
    [Header("Help")]
    public GameObject helpPanel;

    public void StartGame() {
        audioSource.Play();
        title.SetActive(false);
        startButton.SetActive(false);
        leftCurtain.transform.DOMoveX(leftCurtain.transform.position.x - curtainMoveDistance, duration).SetUpdate(true);
        rightCurtain.transform.DOMoveX(rightCurtain.transform.position.x + curtainMoveDistance, duration).SetUpdate(true);
    }

    public void hideHelp() {
        helpPanel.SetActive(false);
        GameManagerFSM.Instance.SetTrigger(AI.FSM.FSMTriggerID.GameStart);
        Destroy(gameObject);
    }
}
