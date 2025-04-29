using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageCooldown = 0.2f;
    private float lastDamageTime = 0f;
    EnemyDeathManager deathManager;
    [SerializeField] LayerMask Target;
    private void Start()
    {
        deathManager = GetComponent<EnemyDeathManager>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (deathManager.isDead) return;
        if (((1 << other.gameObject.layer) & Target.value) != 0)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                ResourceManager playerHealth = other.gameObject.GetComponent<ResourceManager>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}
