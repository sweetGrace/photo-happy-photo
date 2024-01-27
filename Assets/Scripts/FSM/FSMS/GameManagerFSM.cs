using UnityEngine;
using AI.FSM;

public class GameManagerFSM : FSMBase {
    public static GameManagerFSM Instance { get; private set; } = null;

    [Range(0, 600f), Tooltip("Unit: seconds")]
    public float GameTime;
    [HideInInspector]
    public float restTime;
    public int basicScore;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int combo;
    public float comboMultiplier {
        get {
            if (combo < 3)
                return 1f;
            return 1f + (combo - 2) * 0.5f;
        }
    }

    protected override void Init() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogWarning("GameManager instance already exists. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    protected override void SetUpFSM() {
        base.SetUpFSM();

        GameEndState endState = new GameEndState();
        endState.AddMap(FSMTriggerID.GameStart, FSMStateID.GameRunning);
        _states.Add(endState);

        GameRunningState gameRunningState = new GameRunningState();
        gameRunningState.AddMap(FSMTriggerID.GameEnd, FSMStateID.GameEnd);
        _states.Add(gameRunningState);
    }
}
