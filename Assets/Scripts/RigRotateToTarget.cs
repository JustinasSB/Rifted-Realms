using UnityEngine;

public class RigRotateToTarget : MonoBehaviour
{
    [SerializeField] public Transform Target;
    [SerializeField] Transform Model;
    bool dead = false;

    private void Start()
    {
        if (Target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Player object not found in the scene.");
                return;
            }
        }
        EnemyDeathManager deathManager = this.GetComponentInParent<EnemyDeathManager>();
        deathManager.OnDeath += Dead;
        deathManager.OnRevive += Revive;
    }
    private void Dead(GameObject o)
    {
        dead = true;
    }
    private void Revive()
    {
        dead = false;
    }
    void Update()
    {
        if (dead) return;
        Vector3 lookPos = new Vector3(Target.position.x, Model.position.y, Target.position.z);
        if (lookPos - Model.position != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookPos - Model.position);
            Model.rotation = lookRotation;
        }
    }
}
