using System.Collections.Generic;
using FishNet.Object;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private Color color;
    [SerializeField] private bool IsReady = false;
    public TextMeshPro nameText;
    public List<Renderer> meshes;
    public override void OnStartServer()
    {
        base.OnStartServer();
        PlayersReady.Instance.gameObject.SetActive(true);
        PlayersReady.Instance.players.Add(this);
    }
    public bool PlayerReady
    {
        get => IsReady;
        set
        {
            IsReady = value;
            TogglePlayerReadyServer(this, IsReady);
        }
    }
    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            ChangePlayerNameServer(this, playerName);
        }
    }
    public Color Color
    {
        get => color;
        set
        {
            color = value;
            ChangeColorServer(this, color);
        }
    }
    [ServerRpc]
    public void ActivatePlayer(Player player, bool condition)
    {
        ActivatePlayerOnObservers(player, condition);
    }
    [ObserversRpc]
    public void ActivatePlayerOnObservers(Player player, bool condition)
    {
        if (player == null) return;

        print("ActivateChildOnObservers");
        player.GetComponent<PlayerController>().enabled = condition;
        player.GetComponent<CharacterController>().enabled = condition;
        player.GetComponent<HealthController>().enabled = condition;
        player.GetComponent<PlayerShoot>().enabled = condition;
        player.GetComponent<PlayerSpawnObjectController>().enabled = condition;
    }
    [ServerRpc]
    public void TogglePlayerReadyServer(Player player, bool isReady)
    {
        player.IsReady = isReady;
    }
    [ServerRpc]
    public void ChangePlayerNameServer(Player player, string name)
    {
        ChangePlayerName(player, name);
    }
    [ObserversRpc]
    public void ChangePlayerName(Player player, string name)
    {
        player.nameText.text = name;
    }
    [ServerRpc]
    public void ChangeColorServer(Player player, Color color)
    {
        ChangeColor(player, color);
    }
    [ObserversRpc]
    public void ChangeColor(Player player, Color color)
    {
        foreach (Renderer item in player.meshes)
        {
            item.materials[0].color = color;
            item.enabled = true;
        }
    }
}