using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private Image hpImage;

    private float maxHP;
    private float minHP;

    private void Awake() {
        hpImage.fillAmount = 1;
    }
    private void Start() {
        maxHP = Player.Instance.StartScale;
        minHP = Player.Instance.MinScale;

        PathScaler.Instance.OnFreePath += PathScaler_OnFreePath;
    }

    private void PathScaler_OnFreePath(object sender, EventArgs e) {
        gameObject.SetActive(false);
    }

    private void Update() {
        DisplayHP();
    }

    private void DisplayHP() {
        float amount = (Player.Instance.PlayerScaler.localScale.x - minHP) / (maxHP - minHP);
        hpImage.fillAmount = amount;
    }
}
