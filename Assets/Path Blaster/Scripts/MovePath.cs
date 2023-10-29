using System;
using System.Collections;
using UnityEngine;

public class MovePath : MonoBehaviour
{
    static public MovePath Instance { private set; get; }
    private static string PLAYER_JUMP_BOOL = "Jump";

    [SerializeField] private Transform target;
    [SerializeField] private Transform scaler;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 20f;
    public event EventHandler OnPathPassed;

    private float distanceToStartAnimation = 5f;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("MovePath Instance duplication");
        }
        Instance = this;

       
    }

    private void Start() {
        PathScaler.Instance.OnFreePath += PathScaler_OnFreePath;
        distanceToStartAnimation = (distanceToStartAnimation * scaler.localScale.z / 2) + target.localScale.z / 2;
    }

    private void PathScaler_OnFreePath(object sender, System.EventArgs e) {
        StartCoroutine(MoveToTargetCoroutine());
    }

    private void OnDestroy() {
        PathScaler.Instance.OnFreePath -= PathScaler_OnFreePath;
    }

    IEnumerator MoveToTargetCoroutine() {
        animator.SetBool(PLAYER_JUMP_BOOL, true);
        
        float stopPointZ = target.position.z - distanceToStartAnimation;

        float positionDelta = .1f;
        while ((transform.position.z < (stopPointZ + positionDelta)) || (transform.position.z < (stopPointZ - positionDelta))){
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            yield return null;
        }

        animator.SetBool(PLAYER_JUMP_BOOL, false);
        OnPathPassed.Invoke(this, EventArgs.Empty);

        yield break;
    }

}
