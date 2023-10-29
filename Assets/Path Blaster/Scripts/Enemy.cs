using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Material infectedMaterial;
    [SerializeField] private Transform[] enemyComponents;
    public void BecomeInfected() {

        foreach (Transform component in enemyComponents) {
            component.GetComponent<MeshRenderer>().material = infectedMaterial;
        }

        float delayToDestroy = 1f;
        Destroy(gameObject, delayToDestroy);
    }

}
