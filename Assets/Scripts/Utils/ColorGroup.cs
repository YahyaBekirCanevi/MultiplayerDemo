using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGroup : MonoBehaviour
{
    public GameObject gridItem;
    public List<Color> colors;
    private void Start()
    {
        foreach (Color item in colors)
        {
            GameObject newItem = Instantiate(gridItem, Vector3.zero, Quaternion.identity, transform);
            newItem.AddComponent<Button>().onClick.AddListener(() =>
                MainSceneController.Instance.player.Color = item
            );
            newItem.transform.GetChild(0).GetComponent<Image>().color = item;
        }
    }

}
