using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [HideInInspector] public PlayerSpawnObjectController spawnObjectController;
    public DamageDealerObject damageDealerObject;
    private float time = 0;
    private float height = 0;
    private bool directionUp = true;
    private void Update()
    {
        time += Time.deltaTime;
        if (height >= 1 && directionUp) directionUp = false;

        height = damageDealerObject.Movement(damageDealerObject, height, time);
        height *= directionUp ? 1 : -1;
        transform.position +=
            (transform.forward * damageDealerObject.movementSpeed) +
            (Vector3.up * height);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out HealthController healthController))
        {
            healthController.UpdateHealth(healthController, -damageDealerObject.damage);
        }
        spawnObjectController.DespawnObject(damageDealerObject, 0);
    }
}