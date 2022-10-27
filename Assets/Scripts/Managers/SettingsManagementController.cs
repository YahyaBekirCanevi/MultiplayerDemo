using UnityEngine;

public class SettingsManagementController : MonoBehaviour
{
    public static SettingsManagementController Instance { get; private set; }
    [SerializeField] private bool isFullScreen = true;
    ScreenSize screenSize = ScreenSize.s_auto;
    Vector2 currentScreenSize;
    int index = 3;
    private void Awake()
    {
        Instance = this;
        currentScreenSize = new Vector2(Screen.width, Screen.height);
        ScreenSizeExtension.Init(currentScreenSize);
        SetResulation();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SetFullScreen();
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentScreenSize = screenSize.GetScreenSize();
            SetResulation();
        }

    }
    public void SetFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        SetResulation();
    }
    public void SetResulation()
    {
        Screen.SetResolution(((int)currentScreenSize.x), ((int)currentScreenSize.y), isFullScreen);
    }
}
