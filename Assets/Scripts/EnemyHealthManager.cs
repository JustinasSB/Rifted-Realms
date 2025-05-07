using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] Stat Health;
    [SerializeField] Stat CurrentHealth;
    [SerializeField] Stat RegenerationPercentage;
    [SerializeField] EnemyDeathManager deathManager;
    public bool isAlive;
    public void Start()
    {
        deathManager.OnDeath += onDeath;
        deathManager.OnRevive += onRevive;
    }
    private void Update()
    {
        if (Health.Value > CurrentHealth.Value)
        {
            Regenerate(Health, CurrentHealth, RegenerationPercentage);
        }
    }
    public void onRevive()
    {
        isAlive = true;
    }
    public void onDeath(GameObject o)
    {
        isAlive = false;
    }
    private void Regenerate(Stat Total, Stat Current, Stat Percentage)
    {
        float total = Total.Value;
        float current = Current.Value;
        if (total == current) return;
        if (total > current)
        {
            current += total * Percentage.Value * Time.deltaTime;
            if (current > total)
            {
                Current.DirectValueSet(total);
            }
            else
            {
                Current.DirectValueSet(current);
            }
        }
    }
    public void TakeDamage(float value)
    {
        if (deathManager.isDead) return;
        CurrentHealth.DirectValueSet(CurrentHealth.Value - value);
        if (CurrentHealth.Value < 0)
        {
            deathManager.deathTrigger();
        }
    }
    public void LevelUp(int lvl)
    {
        this.Health.PurgeMultiplierList();
        for (int i = 0; i < lvl; i++)
        {
            this.Health.AddMultiplier(1.1f);
        }
    }
}
