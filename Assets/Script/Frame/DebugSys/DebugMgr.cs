using UnityEngine;
using System.Collections;
using System.Text;

public static class CDebugSet 
{
    public static string color_red = "FF0000";
    public static string color_yellow = "FFFF00";
    public static string color_blue = "00FFFF";
    public static string color_green = "00FF00";
    public static string color_main = "F0FFFF";
    public static string SetTextColor(string txt, string color)
    {
        return "<color=#" + color + ">" + txt + " :</color>";
    }
}
