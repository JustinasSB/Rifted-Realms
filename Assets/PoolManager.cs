using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<ProjectilePool, Queue<GameObject>> pools = new();
    private Dictionary<ProjectilePool, List<GameObject>> allObjects = new();
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
    private void AppendAvailable(GameObject expiredProjectile, ProjectilePool projectilePool)
    {
        expiredProjectile.transform.localScale = Vector3.zero;
        pools[projectilePool].Enqueue(expiredProjectile);
    }
}
