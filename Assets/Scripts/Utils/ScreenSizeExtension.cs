using System.Collections.Generic;
using UnityEngine;

public static class ScreenSizeExtension
{
    private static Vector2 deviceScreenSize;
    public static void Init(Vector2 screen) => deviceScreenSize = screen;
    public static ScreenSize[] Values() => new ScreenSize[] {
        ScreenSize.s_auto,
        ScreenSize.s_3840x2160,
        ScreenSize.s_2560x1600,
        ScreenSize.s_1920x1080,
        ScreenSize.s_1360x768
    };
    public static List<string> Names(this ScreenSize[] value)
    {
        List<string> names = new List<string>();
        foreach (ScreenSize size in value)
        {
            names.Add(size.GetName());
        }
        return names;
    }
    public static Vector2 GetScreenSize(this ScreenSize value)
    {
        Vector2 scale = Vector2.zero;
        switch (value)
        {
            case ScreenSize.s_1360x768:
                scale = new Vector2(1360, 768);
                break;
            case ScreenSize.s_1920x1080:
                scale = new Vector2(1920, 1080);
                break;
            case ScreenSize.s_2560x1600:
                scale = new Vector2(2560, 1600);
                break;
            case ScreenSize.s_3840x2160:
                scale = new Vector2(3840, 2160);
                break;
            default:
                scale = deviceScreenSize;
                break;
        }
        return scale;
    }
    public static string GetName(this ScreenSize value)
    {
        Vector2 current = value.GetScreenSize();
        return $"{current.x}x{current.y}";
    }
}
public enum ScreenSize
{
    s_auto, s_1360x768, s_1920x1080, s_2560x1600, s_3840x2160
}