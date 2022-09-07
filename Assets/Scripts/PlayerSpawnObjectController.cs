using System.Collections;
using FishNet.Object;
using UnityEngine;

public class PlayerSpawnObjectController : NetworkBehaviour
{
    public float waitForSecondsToDespawn = 2f;
    [SerializeField] private float waiting;
    public GameObject objectToSpawn;
    [HideInInspector] public GameObject spawnedObject;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!(base.IsOwner))
        {
            GetComponent<PlayerSpawnObjectController>().enabled = false;
        }
    }
    private void Update()
    {
        if (spawnedObject == null && Input.GetKeyDown(KeyCode.Mouse0) && !GameManagementController.Instance.isMenuOpen)
        {
            SpawnObjectOnServer();
        }
    }
    private void SpawnObjectOnServer()
    {
        if (objectToSpawn != null)
        {
            SpawnObject(objectToSpawn, transform, this);
        }
        else print("objectToSpawn is null");
    }

    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObjectController script)
    {
        if (obj != null)
        {
            GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
            if (spawned != null)
            {
                ServerManager.Spawn(spawned);
                SetSpawnedObject(spawned, script);
                DespawnObject(spawned, script);
            }
            else print("spawned is null on server");
        }
        else print("obj is null on server");
    }

    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObjectController script)
    {
        if (spawned != null)
        {
            script.spawnedObject = spawned;
        }
        else print("spawned is null");
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject spawned, PlayerSpawnObjectController script)
    {
        ServerManager.StartCoroutine(DespawnObjectThatSpawned(spawned, script));
    }

    private IEnumerator DespawnObjectThatSpawned(GameObject spawned, PlayerSpawnObjectController script)
    {
        waiting = 0;
        while (waiting < waitForSecondsToDespawn)
        {
            waiting += Time.deltaTime;
            // if (collided)
            //{
            //    break;
            //} 
            yield return new WaitForEndOfFrame();
        }
        ServerManager.Despawn(spawned);
        SetSpawnedObject(null, script);
    }
}