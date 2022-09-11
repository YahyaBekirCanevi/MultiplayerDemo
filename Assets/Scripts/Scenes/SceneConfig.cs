using FishNet.Managing;
using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Utility;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneConfig : MonoBehaviour
{
    public static SceneConfig Instance { get; private set; }
#if UNITY_EDITOR
    public UnityEditor.SceneAsset mainScene;
    public UnityEditor.SceneAsset gameScene;
    private void OnValidate()
    {
        if (mainScene != null) mainSceneName = mainScene.name;
        if (gameScene != null) gameSceneName = gameScene.name;
    }
#endif
    public string mainSceneName;
    public string gameSceneName;
    private LocalConnectionState _clientState = LocalConnectionState.Stopped;
    private LocalConnectionState _serverState = LocalConnectionState.Stopped;
    private NetworkManager _networkManager;
    private bool startedAsHost = false;
    private void Awake()
    {
        Instance = this;
        InitializeOnce();
    }
    private void OnDestroy()
    {
        if (ApplicationState.IsQuitting() && _networkManager != null && _networkManager.Initialized)
        {
            _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
            _networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
            _networkManager.SceneManager.OnLoadEnd -= SceneManager_OnLoadEnd;
        }
    }
    private void InitializeOnce()
    {
        _networkManager = GetComponent<NetworkManager>();
        if (_networkManager == null)
        {
            if (NetworkManager.StaticCanLog(LoggingType.Error))
                Debug.LogError($"Network Manager not found on {gameObject.name} or any parent objects. Default scene will not work.");
            return;
        }
        if (!_networkManager.Initialized)
            return;
        if (mainSceneName == string.Empty || gameSceneName == string.Empty)
        {
            if (_networkManager.CanLog(LoggingType.Error))
                Debug.LogWarning("Scene(s) is not specified. Scenes in SceneConfig will not load");
            return;
        }
        _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        _networkManager.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
    }
    private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        bool onlineLoaded = false;
        foreach (Scene s in obj.LoadedScenes)
        {
            if (s.name == GetSceneName(gameSceneName))
            {
                onlineLoaded = true;
                break;
            }
        }
        if (onlineLoaded)
        {
            UnloadMainScene();
        }
    }
    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        _serverState = obj.ConnectionState;
        if (_serverState == LocalConnectionState.Started)
        {
            if (!_networkManager.ServerManager.OneServerStarted())
                return;

            //If here can load scene.
            /* SceneLoadData sld = new SceneLoadData(GetSceneName(game_sceneName));
            sld.ReplaceScenes = ReplaceOption.All;
            _networkManager.SceneManager.LoadGlobalScenes(sld); */
        }
        //When server stops load offline scene.
        else if (obj.ConnectionState == LocalConnectionState.Stopped)
        {
            LoadMainScene();
        }
    }
    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        _clientState = obj.ConnectionState;

        if (_clientState == LocalConnectionState.Stopped)
        {
            if (!_networkManager.IsServer)
                LoadMainScene();
        }
    }
    public void LoadGameScene()
    {
        SceneLoadData sld = new SceneLoadData(gameSceneName);
        int count = PlayersReady.Instance.players.Count;
        NetworkObject[] players = new NetworkObject[count];
        for (int i = 0; i < count; i++)
        {
            players[i] = PlayersReady.Instance.players[i].GetComponent<NetworkObject>();
        }
        sld.MovedNetworkObjects = players;
        sld.ReplaceScenes = ReplaceOption.All;
        _networkManager.SceneManager.LoadGlobalScenes(sld);
    }
    private void LoadMainScene()
    {
        if (UnitySceneManager.GetActiveScene().name == GetSceneName(mainSceneName))
            return;
        UnitySceneManager.LoadScene(mainSceneName);
    }
    private void UnloadMainScene()
    {
        Scene s = UnitySceneManager.GetSceneByName(GetSceneName(mainSceneName));
        if (string.IsNullOrEmpty(s.name))
            return;

        UnitySceneManager.UnloadSceneAsync(s);
    }
    private string GetSceneName(string fullPath)
    {
        return Path.GetFileNameWithoutExtension(fullPath);
    }
    public void OnClick_Server()
    {
        startedAsHost = false;
        if (_networkManager == null)
            return;

        if (_serverState != LocalConnectionState.Stopped)
            _networkManager.ServerManager.StopConnection(true);
        else
            _networkManager.ServerManager.StartConnection();
    }
    public void OnClick_Client()
    {
        startedAsHost = false;
        if (_networkManager == null)
            return;

        if (_clientState != LocalConnectionState.Stopped)
            _networkManager.ClientManager.StopConnection();
        else
            _networkManager.ClientManager.StartConnection();
    }
    public void OnClick_ClientExit()
    {
        if (_networkManager == null)
            return;
        if (startedAsHost)
            _networkManager.ServerManager.StopConnection(true);
        else
            _networkManager.ClientManager.StopConnection();
    }
    public void OnClick_Host()
    {
        OnClick_Server();
        OnClick_Client();
        startedAsHost = true;
    }
}
