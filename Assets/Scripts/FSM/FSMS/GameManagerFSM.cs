using UnityEngine;
using AI.FSM;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameManagerFSM : FSMBase {
    public static GameManagerFSM Instance { get; private set; } = null;

    [Range(0, 600f), Tooltip("Unit: seconds"), Header("Game Settings")]
    public float GameTime;
    public int basicScore;
    public int happyScore;
    public int photoBasicScore;
    [Header("Default Positions")]
    public Transform defaultPlayerPosition;
    public Item[] items;
    public Transform[] itemPositions;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip comboClip;
    public AudioClip scoreClip;
    [Header("UI")]
    public GameObject gameEndUI;

    public bool cameraed { get; set; } = false;
    [HideInInspector]
    public float restTime;
    private int _score;
    public int Score {
        get { return _score; }
        set {
            if (value > _score && audioSource != null && scoreClip != null)
                audioSource.PlayOneShot(scoreClip);
            _score = value;
        }
    }
    private int _combo;
    public int Combo {
        get { return _combo; }
        set {
            if (value > _combo && audioSource != null && comboClip != null)
                audioSource.PlayOneShot(comboClip);
            _combo = value;
        }
    }
    public float comboMultiplier {
        get {
            if (Combo < 3)
                return 1f;
            return 1f + (Combo - 2) * 1f;
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

    public int GetFinalScore() {
        if (!cameraed)
            return _score;
        int cnt = GameObject.FindGameObjectsWithTag("Pal")
            .Count(obj => obj.GetComponent<PalFSM>().CurrentState != FSMStateID.PalCry);
        // Debug.Log(_score + ", " + cnt + ", " + happyScore + ", " + basicScore);
        return _score + cnt * happyScore + photoBasicScore;
    }
}
