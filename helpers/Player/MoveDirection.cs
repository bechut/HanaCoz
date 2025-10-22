using Godot;

namespace HanaCoz.Helpers.Player;

public static partial class Helper
{
    public static string MoveDirection(Vector2 velocity, string lastDir)
    {
        if (velocity.Length() < 0.1)
            return lastDir;
        if (Mathf.Abs(velocity.X) > Mathf.Abs(velocity.Y))
            return velocity.X > 0 ? "r" : "l";
        return velocity.Y > 0 ? "d" : "u";
    }
}