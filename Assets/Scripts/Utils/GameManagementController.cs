using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManagementController : MonoBehaviour
{
    public static GameManagementController Instance { get; private set; }
    public float lowestPoint = -10;
    [SerializeField] private float gravity = -9.8f;
    public float Gravity { get => gravity; }
    public Transform[] SpawnPoints;
    public GameObject escapeMenu;
    public Slider healthBar;
    public Slider chargeMeter;
    public Player player;
    public bool isMenuOpen = true;
    private void Start()
    {
        Instance = this;
        ToggleMenu(true);
        StartCoroutine(FindAndPlacePlayers());
    }

    public IEnumerator FindAndPlacePlayers()
    {
        Player[] players = null;
        while (players == null)
        {
            players = GameObject.FindObjectsOfType<Player>();
            print("Players : " + players);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log($"Player Count : {players.Length}");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].ActivatePlayer(players[i], true);
            if (players[i].IsOwner)
            {
                player = players[i];
                player.transform.position = SpawnPoints[i % SpawnPoints.Length].position;
                player.transform.rotation = SpawnPoints[i % SpawnPoints.Length].rotation;
            }
        }
    }

    public void ToggleMenu(bool open)
    {
        isMenuOpen = open;
        escapeMenu.SetActive(isMenuOpen);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu(!isMenuOpen);
        }
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
