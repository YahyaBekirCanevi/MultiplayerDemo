using System;
using System.Collections;
using FishNet.Object;
using UnityEngine;

[Serializable]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private float attack = 5;
    [SerializeField] private float critMultiplyer = 2.5f;
    [SerializeField] private float timeToCharge = 3;
    [SerializeField, ReadOnly] private float chargeTimer = 0;
    [SerializeField, ReadOnly] private bool charge;
    [SerializeField] private bool isCharging = false;
    public DamageDealerObject bullet;
    private PlayerSpawnObjectController spawnObjectController;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!(base.IsOwner))
        {
            GetComponent<PlayerShoot>().enabled = false;
        }
    }
    private void OnEnable()
    {
        spawnObjectController = GetComponent<PlayerSpawnObjectController>();
    }
    private void Update()
    {
        if (isCharging && !Input.GetKey(KeyCode.Mouse0)) isCharging = false;
        if (!GameManagementController.Instance.isMenuOpen && Input.GetKeyDown(KeyCode.Mouse0) && !isCharging)
        {
            StartCoroutine(Charge());
        }
        if (GameManagementController.Instance.isMenuOpen)
        {
            StopCoroutine(Charge());
            chargeTimer = 0;
            GameManagementController.Instance.chargeMeter.value = 0;
            GameManagementController.Instance.chargeMeter.gameObject.SetActive(false);
        }
    }
    private IEnumerator Charge()
    {
        charge = false;
        isCharging = true;
        chargeTimer = 0;
        GameManagementController.Instance.chargeMeter.value = 0;
        GameManagementController.Instance.chargeMeter.gameObject.SetActive(true);
        while (isCharging)
        {
            chargeTimer += Time.deltaTime;
            GameManagementController.Instance.chargeMeter.value = Mathf.Clamp(chargeTimer / timeToCharge, 0, 1);
            yield return new WaitForEndOfFrame();
        }
        if (chargeTimer > timeToCharge)
            charge = true;
        chargeTimer = 0;
        GameManagementController.Instance.chargeMeter.value = 0;
        GameManagementController.Instance.chargeMeter.gameObject.SetActive(false);
        Shoot();
        print("shoot");
    }

    public float CalculateDamage()
    {
        float damage = attack;
        damage *= charge ? critMultiplyer : 1;
        return damage;
    }
    private void Shoot()
    {
        if (bullet is DamageDealerObject)
        {
            bullet.damage = CalculateDamage();
            bullet.Movement += Bullet_Movement;
            if (!bullet.objectToSpawn.TryGetComponent(out DealDamage _))
                bullet.objectToSpawn.AddComponent<DealDamage>();
            if (bullet.objectToSpawn.TryGetComponent(out DealDamage dealDamage))
            {
                dealDamage.damageDealerObject = bullet;
                dealDamage.spawnObjectController = spawnObjectController;
                dealDamage.spawnObjectController.DespawnObject(bullet, bullet.waitForSecondsToDespawn);
            }
        }
        spawnObjectController.SpawnObject(
            bullet,
            transform.position + Vector3.up + transform.forward,
            transform.rotation
        );
    }
    public float Bullet_Movement(DamageDealerObject obj, float height, float time)
    {
        return Mathf.Lerp(
            height, 1,
            obj.movemnetCurve.Evaluate(time / obj.waitForSecondsToDespawn)
        );
    }
}