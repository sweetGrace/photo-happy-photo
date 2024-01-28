using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour {
    public Text restTimeText;
    public Text scoreText;
    public Text comboText;

    public void OnGUI() {
        restTimeText.text = (Mathf.Max(0, GameManagerFSM.Instance.restTime)).ToString("0");
        scoreText.text = GameManagerFSM.Instance.Score.ToString();
        comboText.text = GameManagerFSM.Instance.Combo.ToString();
    }
}
