using System.Collections;
using FishNet.Object;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private float attack = 5;
    [SerializeField] private float critMultiplyer = 2.5f;
    [SerializeField] private float damage;
    [SerializeField] private float timeToCharge = 3;
    [SerializeField] private float chargeTimer = 0;
    [SerializeField] private bool charge;
    private void Update()
    {
        if (!base.IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Charge());
        }
    }
    private IEnumerator Charge()
    {
        chargeTimer = 0;
        while (true)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
                if (chargeTimer > timeToCharge)
                {
                    charge = true;
                    break;
                }

            chargeTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        DealDamage();
    }

    private void DealDamage()
    {
        damage = attack * (charge ? critMultiplyer : 1);
        ShootServer(damage, Camera.main.transform.position, Camera.main.transform.forward);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootServer(float damagetoDeal, Vector3 position, Vector3 direction)
    {
        if (Physics.Raycast(position, direction, out RaycastHit hit)
            && hit.transform.TryGetComponent(out HealthController healthController))
        {
            healthController.UpdateHealth(healthController, -damagetoDeal);
        }
    }
}