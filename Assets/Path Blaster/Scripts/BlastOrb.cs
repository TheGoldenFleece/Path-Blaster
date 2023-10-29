using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlastOrb : MonoBehaviour
{
    private const string ENEMY_LAYER_NAME = "Enemy";

    [SerializeField] private ParticleSystem explosionVFXPrefab;
    [SerializeField] [Range(50f, 100f)] private float speed = 10f;

    private float timeToLive = 2.5f;
    public void ChangeScale(float delta) {
        Vector3 scale = new Vector3(transform.localScale.x + delta * Time.deltaTime, transform.localScale.y + delta * Time.deltaTime, transform.localScale.z + delta * Time.deltaTime);

        transform.localScale = scale;
    }

    public void Shoot() {
        StartCoroutine(ShootCoroutine());
        Destroy(gameObject, timeToLive);
    }

    IEnumerator ShootCoroutine() {
        while (!GameManager.Instance.IsGameOver) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            yield return null;
        }

        yield break;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer(ENEMY_LAYER_NAME)) return;
        Vector3 explosionEpicenter = other.ClosestPoint(transform.position);

        Explode(explosionEpicenter);
    }

    public void Explode(Vector3 explosionEpicenter) {
        ParticleSystem explosion = Instantiate(explosionVFXPrefab, explosionEpicenter, Quaternion.identity);

        float explosionRadius = transform.localScale.z;

        explosion.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);

        int maxColliders = 50;
        Collider[] hitColliders = new Collider[maxColliders];

        int enemyLayer = LayerMask.GetMask(ENEMY_LAYER_NAME);
        int numColliders = Physics.OverlapSphereNonAlloc(explosionEpicenter, explosionRadius, hitColliders, enemyLayer);

        List<Transform> hitEnemyList = GetFilteredInfectedTransform(hitColliders, numColliders);
        foreach (Transform t in hitEnemyList) {
            Enemy enemy = t.GetComponent<Enemy>();
            enemy.BecomeInfected();
        }

        float delayToDestroyVFX = 2f;

        Destroy(explosion.gameObject, delayToDestroyVFX);
        Destroy(gameObject);
    }

    private List<Transform> GetFilteredInfectedTransform(Collider[] colliders, int numColliders) {
        List<Transform> filteredTransforms = new List<Transform>();

        for (int i = 0; i < numColliders; i++) {
            Transform t = colliders[i].transform.parent;

            bool found = filteredTransforms.Any(item => item.gameObject.GetInstanceID() == t.gameObject.GetInstanceID());

            if (!found) {
                filteredTransforms.Add(t);
            }
        }

        return filteredTransforms;
    }
}
