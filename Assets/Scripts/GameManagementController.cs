using UnityEngine;
using UnityEngine.UI;

public class GameManagementController : MonoBehaviour
{
    public static GameManagementController Instance { get; private set; }
    public float lowestPoint = -10;
    public GameObject menu;
    public bool isMenuOpen = true;
    public Color selectedColor;
    public Slider playerHealthSlider;
    private void Start()
    {
        Instance = this;
        ToggleMenu(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu(!isMenuOpen);
        }
    }
    public void ToggleMenu(bool open)
    {
        isMenuOpen = open;
        menu.SetActive(isMenuOpen);
    }
    public void ChangeColor(Image image)
    {
        selectedColor = image.color;
        PlayerColorNetwork.Instance.GetSelectedColor(selectedColor);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
