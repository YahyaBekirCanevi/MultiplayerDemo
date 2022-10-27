using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [HideInInspector] public SpawnObjectController spawnObjectController;
    public DamageDealerObject damageDealerObject;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out HealthController healthController))
        {
            healthController.UpdateHealth(healthController, -damageDealerObject.damage);
        }
        spawnObjectController.DespawnObject(damageDealerObject, 0);
    }
}