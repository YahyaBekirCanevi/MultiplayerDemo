using System.Collections;
using FishNet.Object;
using UnityEngine;

public class SpawnObjectController : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!(base.IsOwner))
        {
            GetComponent<SpawnObjectController>().enabled = false;
        }
    }

    [ServerRpc]
    public void SpawnObject(SpawnableObject obj, Vector3 position, Quaternion rotation)
    {
        if (obj.objectToSpawn != null)
        {
            GameObject spawned = Instantiate(obj.objectToSpawn, position, rotation);
            if (spawned != null)
            {
                ServerManager.Spawn(spawned);
                SetSpawnedObject(obj, spawned);
            }
            else print("spawned is null on server");
        }
        else print("obj is null on server");
    }

    [ObserversRpc]
    public void SetSpawnedObject(SpawnableObject obj, GameObject spawned)
    {
        obj.spawned = spawned;
        if (spawned == null) print("spawned is null");
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(SpawnableObject obj, float time)
    {
        ServerManager.StartCoroutine(DespawnObjectThatSpawned(obj, time));
    }

    private IEnumerator DespawnObjectThatSpawned(SpawnableObject obj, float time)
    {
        if (time > 0)
        {
            float waiting = 0;
            while (waiting < time)
            {
                waiting += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        SetSpawnedObject(obj, null);
        ServerManager.Despawn(obj.spawned);
    }
}