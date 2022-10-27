using UnityEngine;

[CreateAssetMenu(fileName = "DamageDealerObject", menuName = "DamageDealerObject/New")]
public class DamageDealerObject : SpawnableObject
{
    public float damage;
    public float timeToCharge = 3;
    public float movementSpeed;
    public DamageDealerObject(float _waitForSecondsToDespawn, GameObject _objectToSpawn, float _damage, float _movementSpeed)
        : base(_waitForSecondsToDespawn, _objectToSpawn)
    {
        this.damage = _damage;
        this.movementSpeed = _movementSpeed;
    }
}