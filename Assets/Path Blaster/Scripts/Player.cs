using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {

    static public Player Instance { private set; get; }

    [SerializeField] private Transform blastOrbPrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private Transform playerScaler;
    public Transform PlayerScaler => playerScaler;
    [SerializeField] [Range(1f, 100f)] private float startScale = 20f;
    public float StartScale => startScale;

    [SerializeField] [Range(1f, 5f)] private float scalingSpeed = 0.1f;
    [SerializeField] [Range(1f, 5f)] private float scalingCoefficient = 2f;

    [SerializeField][Range(1f, 10f)] private float minScale = 1f;
    public float MinScale => minScale;

    public event EventHandler OnScaleChanged;

    private IEnumerator scalingCoroutine;
    private BlastOrb blastOrb;

    private InputActions inputActions;
    private PlayerInput playerInput;
    private bool enablePlayerInput;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Player Instance duplication");
        }
        Instance = this;

        playerScaler.localScale = new Vector3(startScale, startScale, startScale);

        scalingCoroutine = ScalingCoroutine();

        enablePlayerInput = true;
        playerInput = GetComponent<PlayerInput>();
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    private void Start() {
        playerInput.actions["Shoot"].started += OnTabStarted;
        playerInput.actions["Shoot"].canceled += OnTabCanceled;

        GameManager.Instance.OnGameOver += GameOver_OnGameOver;
        PathScaler.Instance.OnFreePath += PathScaler_OnFreePath;
    }

    private void PathScaler_OnFreePath(object sender, EventArgs e) {
        StopCoroutine(scalingCoroutine);
        if (blastOrb != null) Destroy(blastOrb.gameObject);
        OnDestroy();
        this.enabled = false;
    }

    private void GameOver_OnGameOver(object sender, EventArgs e) {
        this.enabled = false;
    }

    private void OnDestroy() {
        playerInput.actions["Shoot"].started -= OnTabStarted;
        playerInput.actions["Shoot"].canceled -= OnTabCanceled;

        GameManager.Instance.OnGameOver -= GameOver_OnGameOver;
        PathScaler.Instance.OnFreePath -= PathScaler_OnFreePath;
    }

    private void OnTabStarted(InputAction.CallbackContext obj) {
        enablePlayerInput = !IsPointerOverUIObject() && !IsPointerOverUIObject() && (Time.timeScale == 1f);

        if (!enablePlayerInput) return; 

        SpawnBlastOrb();
        StartCoroutine(scalingCoroutine);
    }
    private void OnTabCanceled(InputAction.CallbackContext obj) {
        if (!enablePlayerInput) return;

        StopCoroutine(scalingCoroutine);
        blastOrb.Shoot();
    }

    private void SpawnBlastOrb() {
        Transform blastOrbTransform = Instantiate(blastOrbPrefab, shootPosition.position, Quaternion.identity);
        blastOrb = blastOrbTransform.GetComponent<BlastOrb>();
    }

    IEnumerator ScalingCoroutine() {
        while (!IsMinimalScale(playerScaler.localScale) && !GameManager.Instance.IsGameOver) {
            Vector3 scale = playerScaler.localScale - new Vector3(scalingSpeed, scalingSpeed, scalingSpeed) * minScale * Time.deltaTime;

            playerScaler.localScale = scale;

            blastOrb.ChangeScale(scalingSpeed * scalingCoefficient * minScale);

            yield return null;
        }

        yield break;
    }

    private bool IsMinimalScale(Vector3 scale) {

        if (scale.x >= minScale) return false;
        if (scale.y >= minScale) return false;
        if (scale.z >= minScale) return false;

        return true;
    }


    private bool IsPointerOverUIObject() {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private bool IsTouchOverUIObject() {
        foreach (var touch in Input.touches) {
            if (touch.phase == UnityEngine.TouchPhase.Began) {
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = touch.position;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
                if (results.Count > 0) {
                    return true;
                }
            }
        }
        return false;
    }
}


