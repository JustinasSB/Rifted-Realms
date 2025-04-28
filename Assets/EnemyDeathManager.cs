using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyDeathManager : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private string propertyName = "_Step";
    [SerializeField] private float startValue = 0f;
    [SerializeField] private float endValue = 1f;
    [SerializeField] private float duration = 1f;
    [SerializeField] SkinnedMeshRenderer[] objects;
    public event Action OnDeath;
    public int level;
    private void Start()
    {
        level = LevelManager.level.CurrentLevel + UnityEngine.Random.Range(0, 4);
        EnemyHealthManager healthManager = GetComponent<EnemyHealthManager>();
        healthManager.LevelUp(level);
    }
    public void deathTrigger()
    {
        LevelManager.level.IncreaseExperience((ulong)(20 * level), level);
        foreach (var obj in objects)
        {
            obj.material = targetMaterial;
        }
        OnDeath?.Invoke();
        StartCoroutine(AnimateShaderProperty());
    }
    private IEnumerator AnimateShaderProperty()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            targetMaterial.SetFloat(propertyName, currentValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetMaterial.SetFloat(propertyName, endValue);
        Destroy(gameObject);
    }
}
