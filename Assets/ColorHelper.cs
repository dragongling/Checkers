using System;
using UnityEngine;

public static class ColorHelper
{
    public static Color Invert(Color source)
    {
        var colorValue = ColorToInt(source);
        colorValue = colorValue ^ int.MaxValue;
        var newColor = IntToColor(colorValue);
        newColor.a = 1;
        return newColor;
    }

    public static int ColorToInt(Color color)
    {
        Color32 color32 = color;
        byte[] bytes = { color32[0], color32[1], color32[2], color32[3] };
        return BitConverter.ToInt32(bytes);
    }

    public static Color IntToColor(int colorValue)
    {
        var bytes = BitConverter.GetBytes(colorValue);
        Color32 color32 = new Color32();
        for (int i = 0; i < 4; ++i)
        {
            color32[i] = bytes[i];
        }
        return color32;
    }
}
