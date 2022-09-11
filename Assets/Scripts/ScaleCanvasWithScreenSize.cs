using UnityEngine;
using UnityEngine.UI;

public class ScaleCanvasWithScreenSize : MonoBehaviour
{
    public static ScaleCanvasWithScreenSize Instance { get; private set; }
    private CanvasScaler canvasScaler;
    public MatchType matchType = MatchType.width;
    [ReadOnly] public Vector2 screenSize;
    private void Awake()
    {
        Instance = this;
        canvasScaler = GetComponent<CanvasScaler>();
    }
    public void UpdateScale()
    {
        canvasScaler.scaleFactor = GetScaleFactor();
    }

    private float GetScaleFactor()
    {
        screenSize = SettingsManagementController.Instance.screenSize;
        return matchType == MatchType.width ? (screenSize.x / 1920) : (screenSize.y / 1080);
    }
}
public enum MatchType { width, height }