using System.Collections;
using TMPro;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public static MainSceneController Instance { get; private set; }
    [ReadOnly] public bool firstScene = true;
    public Camera camera1, camera2;
    public GameObject scene1, scene2;
    public TMP_InputField playerName;
    public ColorGroup colorGroup;
    public Player player;
    private void Awake()
    {
        Instance = this;
    }
    public void SwitchScene()
    {
        firstScene = !firstScene;
        camera1.gameObject.SetActive(firstScene);
        camera2.gameObject.SetActive(!firstScene);
        scene1.SetActive(firstScene);
        scene2.SetActive(!firstScene);
        StartCoroutine(WaitForPlayer(this));
    }

    private IEnumerator WaitForPlayer(MainSceneController script)
    {
        while (script.player == null)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();
            foreach (Player item in players)
            {
                if (item.IsOwner)
                {
                    script.player = item;
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        script.player.PlayerName = script.playerName.text;
    }

    public void Ready()
    {
        player.TogglePlayerReadyServer(player, !player.PlayerReady);
    }
}
