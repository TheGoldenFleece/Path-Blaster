using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinGameUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private void Start() {
        GameManager.Instance.OnVictory += GameManager_OnVictory;

        gameObject.SetActive(false);
    }

    private void OnEnable() {
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(Exit);
    }

    private void OnDisable() {
        restartButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy() {
        GameManager.Instance.OnVictory -= GameManager_OnVictory;
    }
    private void GameManager_OnVictory(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Exit() {
        Application.Quit();
    }
}
