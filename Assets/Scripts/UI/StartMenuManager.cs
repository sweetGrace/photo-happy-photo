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

    public void StartGame() {
        audioSource.Play();
        leftCurtain.transform.DOMoveX(leftCurtain.transform.position.x - curtainMoveDistance, duration);
        rightCurtain.transform.DOMoveX(leftCurtain.transform.position.x + curtainMoveDistance, duration).OnComplete(() => {
            GameManagerFSM.Instance.SetTrigger(AI.FSM.FSMTriggerID.GameStart);
            Destroy(this.gameObject);
        });
    }
}
