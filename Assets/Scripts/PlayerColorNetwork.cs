using FishNet.Object;
using UnityEngine;

public class PlayerColorNetwork : NetworkBehaviour
{
    public static PlayerColorNetwork Instance { get; private set; }
    [SerializeField] private Color endColor;
    [SerializeField] private GameObject body;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {

        }
        else
        {
            gameObject.GetComponent<PlayerColorNetwork>().enabled = false;
        }
    }

    private void Start()
    {
        Instance = this;
    }

    public void GetSelectedColor(Color color)
    {
        endColor = color;
        ChangeColorServer(gameObject, endColor);
    }

    [ServerRpc]
    public void ChangeColorServer(GameObject player, Color color)
    {
        ChangeColor(player, color);
    }
    [ObserversRpc]
    public void ChangeColor(GameObject player, Color color)
    {
        transform.GetComponentInChildren<MeshRenderer>().materials[0].color = color;
    }
}
