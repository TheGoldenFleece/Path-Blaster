using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance { private set; get; }

    [SerializeField] private Button menuButton;

    public bool IsGameOver { private set; get; }

    public event EventHandler OnGameOver;
    public event EventHandler OnVictory;
    public event EventHandler OnGamePaused;
    public event EventHandler OnMenuEnabled;

    private float minScale;
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("GameManager Instance duplication");
        }
        Instance = this;

        IsGameOver = false;
    }

    private void OnEnable() {
        menuButton.onClick.AddListener(PauseGame);
    }

    private void OnDisable() {
        menuButton.onClick.RemoveAllListeners();
    }

    private void Start() {
        Time.timeScale = 1.0f;
        minScale = Player.Instance.MinScale;
    }

    private void Update() {
        if (IsMinimalScale(Player.Instance.PlayerScaler.localScale)) {
            GameOver();
        }
    }

    private bool IsMinimalScale(Vector3 scale) {
        if (scale.x >= minScale) return false;
        if (scale.y >= minScale) return false;
        if (scale.z >= minScale) return false;

        return true;
    }

    private void PauseGame() {
        OnGamePaused.Invoke(this, EventArgs.Empty);
        Time.timeScale = 0.0f;
    }

    private void GameOver() {
        Time.timeScale = 0.0f;
        OnGameOver.Invoke(this, EventArgs.Empty);
        IsGameOver = true;
        this.enabled = false;
    }

    public void Victory() {
        Time.timeScale = 0.0f;
        OnVictory.Invoke(this, EventArgs.Empty);
        IsGameOver = true;
        this.enabled = false;
    }
}
