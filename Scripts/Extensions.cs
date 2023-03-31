using System.Diagnostics;
using Godot;

namespace BasicPlatformController.Scripts;

public static class Extensions{
    public static Vector2 GetRaw(this Vector2 vector){
        vector.X = vector.X switch{
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
        vector.Y = vector.Y switch{
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
        return vector;
    }
}