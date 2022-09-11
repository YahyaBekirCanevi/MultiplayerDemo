using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using FishNet;

public class PlayersReady : NetworkBehaviour
{
    public static PlayersReady Instance { get; private set; }
    [SerializeField] private bool started = false;
    [SerializeField] private bool allPlayersReady = false;
    [SyncObject] public readonly SyncList<Player> players = new SyncList<Player>();
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (!InstanceFinder.NetworkManager.ServerManager.OneServerStarted() || !this.IsServer) return;
        allPlayersReady = players.Count > 0;
        foreach (Player player in players)
        {
            if (!player.PlayerReady)
            {
                allPlayersReady = false;
                break;
            }
        }
        if (allPlayersReady && !started)
        {
            StartCoroutine("StartGame");
        }
        else if (!allPlayersReady && started)
        {
            started = false;
            StopCoroutine("StartGame");
        }
    }
    private IEnumerator StartGame()
    {
        float duration = 2;
        started = true;
        Debug.LogWarning($"{players.Count} Players are READY");
        while (duration > 0)
        {
            Debug.LogWarning($"Game is starting in {duration} second(s) !");
            yield return new WaitForSeconds(1);
            duration--;
        }
        SceneConfig.Instance.LoadGameScene();
        Debug.LogWarning("The game is started!");
    }
}