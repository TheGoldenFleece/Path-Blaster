using UnityEngine;

public class EnemiesSpawner : MonoBehaviour {
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform enemiesContainer;

    [SerializeField] [Range(0f, 1f)] private float enemiesProbability;
    [SerializeField] [Range(1f, 10f)] private float coefficient = 4;
    private Vector2 enemySize = new Vector2(12f, 12f);

    private void Awake() {
        enemySize = new Vector2(enemySize.x / coefficient, enemySize.y / coefficient);
        coefficient /= coefficient;
        SpawnEnemies();
    }

    private void SpawnEnemies() {

        int maxEnemiesZ = (int)((endPoint.position.z - startPoint.position.z) / enemySize.x);

        int maxEnemiesX = (int)((endPoint.position.x - startPoint.position.x) / enemySize.y);

        Vector2 startSpawnPosition = new Vector3(startPoint.position.x + enemySize.x / 2, startPoint.position.z + enemySize.y);

        for (int i = 0; i < maxEnemiesX; i++) {
            for (int j = 0; j < maxEnemiesZ; j++) {
                float random = Random.Range(0f, 1f);

                if (random < enemiesProbability) {
                    Vector3 posToSpawn = new Vector3(startSpawnPosition.x + i * enemySize.x, startPoint.position.y, startSpawnPosition.y + j * enemySize.y);
                    Transform enemy = Instantiate(enemyPrefab, posToSpawn, enemyPrefab.rotation);

                    enemy.SetParent(enemiesContainer);
                }
            }
        }
    }
}
