using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class EnemyDeathManager : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private string propertyName = "_Step";
    [SerializeField] private float startValue = 0f;
    [SerializeField] private float endValue = 1f;
    [SerializeField] private float duration = 1f;
    [SerializeField] SkinnedMeshRenderer[] objects;
    Material clone;
    public event Action<GameObject> OnDeath;
    public event Action OnRevive;
    public bool isDead = false;
    public int level;
    private Material[][] originalMaterials;
    private void Start()
    {
        level = LevelManager.level.CurrentLevel + UnityEngine.Random.Range(0, 4);
        EnemyHealthManager healthManager = GetComponent<EnemyHealthManager>();
        healthManager.LevelUp(level);
        originalMaterials = new Material[objects.Length][];
        for (int i = 0; i < objects.Length; i++)
        {
            originalMaterials[i] = objects[i].materials;
        }
        clone = new Material(targetMaterial);
    }
    public void deathTrigger()
    {
        LevelManager.level.IncreaseExperience((ulong)(20 * level), level);
        foreach (var obj in objects)
        {
            obj.material = clone;
        }
        OnDeath?.Invoke(this.gameObject);
        isDead = true;
        DropItems();
        StartCoroutine(AnimateShaderProperty());
    }
    private void DropItems()
    {
        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand > 0.3)
        {
            Inventory.Singleton.SpawnInventoryItem();
        }
        if (rand > 0.4)
        {
            Inventory.Singleton.SpawnAbility();
        }
    }
    public void Revive()
    {
        isDead = false;
        level = LevelManager.level.CurrentLevel + UnityEngine.Random.Range(0, 4);
        EnemyHealthManager healthManager = GetComponent<EnemyHealthManager>();
        healthManager.LevelUp(level);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].materials = originalMaterials[i];
        }
        OnRevive?.Invoke();
    }
    private IEnumerator AnimateShaderProperty()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            clone.SetFloat(propertyName, currentValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        clone.SetFloat(propertyName, endValue);
        this.gameObject.SetActive(false);
        PoolManager.Instance.EnqueueToHostilePool(gameObject);
    }
}
