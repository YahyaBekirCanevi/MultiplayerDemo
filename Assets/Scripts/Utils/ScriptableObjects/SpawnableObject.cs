using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableObject", menuName = "SpawnableObject/New")]
public class SpawnableObject : ScriptableObject
{
    public float waitForSecondsToDespawn = 10;
    public GameObject objectToSpawn;
    [ReadOnly] public GameObject spawned;

    public SpawnableObject()
    {
        this.waitForSecondsToDespawn = 10;
        this.objectToSpawn = null;
        this.spawned = null;
    }

    public SpawnableObject(float _waitForSecondsToDespawn, GameObject _objectToSpawn)
    {
        this.waitForSecondsToDespawn = _waitForSecondsToDespawn;
        this.objectToSpawn = _objectToSpawn;
        this.spawned = null;
    }
}