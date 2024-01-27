using UnityEngine;

public class Config : MonoBehaviour {
    [Tooltip("Unit: seconds")]
    public float palStateUpdateInterval;

    public static Config Instance { get; private set; } = null;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogWarning("Config already exists, destroying this one.");
            Destroy(gameObject);
        }
    }
}
