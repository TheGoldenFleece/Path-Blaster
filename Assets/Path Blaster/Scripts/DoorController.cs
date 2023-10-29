using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private static string OPEN_DOOR_TRIGGER = "Open";
    [SerializeField] private Animator animator;

    private void Start() {
        MovePath.Instance.OnPathPassed += MovePath_OnPathPassed;
    }

    private void MovePath_OnPathPassed(object sender, EventArgs e) {
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine() {
        animator.SetTrigger(OPEN_DOOR_TRIGGER);
        float delay = 2f;
        yield return new WaitForSeconds(delay);

        GameManager.Instance.Victory();

        yield break;
    }
}
