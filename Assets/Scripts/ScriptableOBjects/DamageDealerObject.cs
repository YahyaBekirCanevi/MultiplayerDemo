using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageDealerObject", menuName = "ScriptableObject/DamageDealerObject")]
public class DamageDealerObject : SpawnableObject
{
    public float damage;
    public float movementSpeed;
    public Func<DamageDealerObject, float, float, float> Movement;
    public AnimationCurve movemnetCurve;
    public DamageDealerObject(float _waitForSecondsToDespawn, GameObject _objectToSpawn, float _damage, float _movementSpeed)
        : base(_waitForSecondsToDespawn, _objectToSpawn)
    {
        this.damage = _damage;
        this.movementSpeed = _movementSpeed;
    }
}