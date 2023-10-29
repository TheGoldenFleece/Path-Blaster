using System;
using System.Collections.Generic;
using UnityEngine;
    
public class PathScaler : MonoBehaviour
{
    static public PathScaler Instance { private set; get; }

    private const string ENEMY_LAYER_NAME = "Enemy";

    [SerializeField] private Transform scaler;
    [SerializeField] private Transform startObject;
    [SerializeField] private Transform endObject;
    [SerializeField] private float jumpHeight = 15f;

    public event EventHandler OnFreePath;

    private Vector3 initialScale;
    private float distance;
    private bool oneTimeWorked;
    private bool isPassBlocked;

    private BoxCollider boxCollider;

    int enemyLayer;
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("PathScaler Instance duplication");
        }
        Instance = this;

        oneTimeWorked = false;

        isPassBlocked = true;

        boxCollider = GetComponent<BoxCollider>();

        initialScale = transform.localScale;

        enemyLayer = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
    }
    private void Start() {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, EventArgs e) {
        this.enabled = false;
    }

    private void FixedUpdate() {
        UpdateTransformScale();

        if (!isPassBlocked && !oneTimeWorked) {

            oneTimeWorked = true;
            OnFreePath.Invoke(this, EventArgs.Empty);
        }

        isPassBlocked = false;
    }

    private void UpdateTransformScale() {
        distance = Vector3.Distance(startObject.position, endObject.position);

        float scaleZ = distance;
        float scaleX = scaler.localScale.x;
        transform.localScale = new Vector3(scaleX, initialScale.y, scaleZ);

        boxCollider.size = new Vector3(boxCollider.size.x, scaler.localScale.y + jumpHeight, boxCollider.size.z);
        boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.size.y / 2, boxCollider.center.z);
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer != enemyLayer) return;

        isPassBlocked = true;
    }

}
