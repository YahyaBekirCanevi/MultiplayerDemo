using UnityEngine;

public class SettingsManagementController : MonoBehaviour
{
    [SerializeField] private bool isFullScreen = true;
    [SerializeField] private Vector2 screenSize;
    private void Awake()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
    }
    public void SetFullScreen()
    {
        isFullScreen = !isFullScreen;
        SetResulation();
    }
    public void SetResulation()
    {
        Screen.SetResolution(((int)screenSize.x), ((int)screenSize.y), isFullScreen);
    }
}
