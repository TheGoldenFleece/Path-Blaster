using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private void Start() {
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        resumeButton.onClick.AddListener(() => {
            Resume();
        });
        restartButton.onClick.AddListener(() => {
            Restart();
        });
        exitButton.onClick.AddListener(() => {
            Exit();
        });

        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy() {
        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
    }

    private void Resume() {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Exit() {
        Application.Quit();
    }
}
