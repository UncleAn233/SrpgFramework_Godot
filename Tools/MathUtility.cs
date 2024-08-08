using Godot;
using System;
using System.Numerics;

namespace SrpgFramework.Utilities
{
    public class MathUtility
    {
        public static Vector2I StringToVector2I(string str)
        {
            var array = str.Replace("(", "").Replace(")", "").Split(",");
            return new Vector2I(int.Parse(array[0]), int.Parse(array[1]));
        }
    }
}