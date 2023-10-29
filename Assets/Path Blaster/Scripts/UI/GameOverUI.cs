using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private void Start() {

        gameObject.SetActive(false);
    }

    private void OnEnable() {
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(Exit);

        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }
    private void OnDisable() {
        restartButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }
    private void GameManager_OnGameOver(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Exit() {
        Application.Quit();
    }
}
