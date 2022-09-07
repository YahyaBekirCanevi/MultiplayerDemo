using UnityEngine;

public class SettingsManagementController : MonoBehaviour
{
    [SerializeField] private bool isFullScreen = true;
    [SerializeField] private Vector2 screenSize;
    private Vector2 scale = new Vector2(16, 9);
    private float[] sizes = new float[] { 600, 720, 1080, 1600, 2160 };
    int index = 3;
    private void Awake()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        SetResulation();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SetFullScreen();
        if (Input.GetKeyDown(KeyCode.R))
        {
            index++;
            index = index % sizes.Length;
            print(index + "" + sizes.Length);
            screenSize = scale * (sizes[index] / scale.y);
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
        Screen.SetResolution(((int)screenSize.x), ((int)screenSize.y), isFullScreen);
    }
}
