using Godot;

namespace HanaCoz.Helpers.Player;

public static partial class Helper
{
  public static Vector2 InputVector()
  {
    Vector2 vel = new();
    if (Input.IsActionPressed("ui_right"))
      vel.X += 1;
    if (Input.IsActionPressed("ui_left"))
      vel.X -= 1;
    if (Input.IsActionPressed("ui_up"))
      vel.Y -= 1;
    if (Input.IsActionPressed("ui_down"))
      vel.Y += 1;

    return vel.Normalized();
  }
}