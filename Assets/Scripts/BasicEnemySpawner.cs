using System.Collections;
using UnityEngine;

public class BasicEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;
    [SerializeField] private Transform player;
    [SerializeField] private float minDistanceFromCenter = 3f;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = Vector3.zero;
        int maxAttempts = 10;
        int attempts = 0;
        bool found = false;
        while (attempts < maxAttempts)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );
            Vector3 candidatePos = transform.position + spawnOffset + randomPos;
            if (Vector3.Distance(candidatePos, player.position) >= minDistanceFromCenter)
            {
                spawnPosition = candidatePos;
                found = true;
                break;
            }
            attempts++;
        }
        if (found)
        {
            GameObject enemy = PoolManager.Instance.GetEnemyObject(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + spawnOffset, spawnAreaSize);
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, minDistanceFromCenter);
        }
    }
}
