using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<ProjectilePool, Queue<GameObject>> pools = new();
    private Dictionary<ProjectilePool, List<GameObject>> allObjects = new();
    private Queue<GameObject> HostilePool = new Queue<GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    public GameObject getGameObject(GameObject prefab, Vector3 origin, quaternion rotation, ProjectilePool projectilePool)
    {
        if (!pools.ContainsKey(projectilePool))
        {
            pools[projectilePool] = new Queue<GameObject>();
            allObjects[projectilePool] = new List<GameObject>();
        }
        Queue<GameObject> pool = pools[projectilePool];
        if (pool.Count > 0)
        {
            GameObject pooledObject = pool.Dequeue();
            pooledObject.transform.SetPositionAndRotation(origin, rotation);
            pooledObject.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
            return pooledObject;
        }
        GameObject instance = Instantiate(
                prefab,
                origin,
                quaternion.identity,
                this.transform
            );
        instance.GetComponent<IProjectile>().OnExpired += AppendAvailable;
        allObjects[projectilePool].Add(instance);
        return instance;
    }
    public GameObject GetEnemyObject(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        GameObject enemy;

        if (HostilePool.Count > 0)
        {
            enemy = HostilePool.Dequeue();
            enemy.transform.SetPositionAndRotation(position, rotation);
            EnemyDeathManager deathManager = enemy.GetComponent<EnemyDeathManager>();
            if (deathManager != null)
            {
                deathManager.Revive();
            }
        }
        else
        {
            enemy = Instantiate(enemyPrefab, position, rotation, this.transform);
            enemy.GetComponent<EnemyDeathManager>().OnDeath += EnqueueToHostilePool;
        }
        enemy.SetActive(true);
        return enemy;
    }
    private void AppendAvailable(GameObject expiredProjectile, ProjectilePool projectilePool)
    {
        expiredProjectile.transform.localScale = Vector3.zero;
        pools[projectilePool].Enqueue(expiredProjectile);
    }
    public void EnqueueToHostilePool(GameObject enemy)
    {
        if (!HostilePool.Contains(enemy))
        {
            HostilePool.Enqueue(enemy);
        }
    }
}
