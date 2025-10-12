using Godot;
namespace HanaCoz.Helpers.UI;
public static class PanelAnimation
{
    public static void Play(Control panel, bool isOpen, double duration = 0.25, float offsetY = -20f)
    {
        if (panel == null)
        {
            return;
        }
        
        var tree = panel.GetTree();
        if (tree == null)
        {
            GD.PrintErr("❌ PanelAnimation.Play: panel is not in the scene tree yet!");
            return;
        }

        var tween = tree.CreateTween();
        panel.Visible = true;

        if (isOpen)
        {
            panel.Scale = new Vector2(0f, 0f);
            panel.Modulate = new Color(1, 1, 1, 0f);

           

            tween.TweenProperty(panel, "modulate:a", 1f, duration * 0.4) // Use 40% of duration for a quick fade
                .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
            tween.TweenProperty(panel, "scale", new Vector2(1f,1f), duration * 0.8) // Use 80% of duration for scale
                .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ);
             
        }
        else
        {
            tween.TweenProperty(panel, "scale", new Vector2(0,0), duration)
                .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Circ);
            tween.TweenProperty(panel, "modulate:a", 0f, duration * 0.2) // Quick fade out (20% duration)
                .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
            tween.Finished += () => panel.Visible = false;
        }
    }
}
