using System;
using UnityEngine;

namespace _Game.Scripts.Utility.Extension
{
    public static class ColorExtension
    {
        public static readonly Color darkGray = ParseColor("#9F9F9F");
        public static readonly Color primary = ParseColor("#009091");

        public static Color ParseColor(string hexColor)
        {
            if (ColorUtility.TryParseHtmlString(hexColor, out var color))
            {
                return color;
            }

            throw new ArgumentException($"Incorrect hex value: {hexColor}");
        }
        
        public static Color SetAlphaChannel(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }
    }
}