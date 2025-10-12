using Godot;
using HanaCoz.Helpers.Signal;

namespace HanaCoz.Scripts.UI;
public partial class InteractiveUi : Control
{
    private Control _hint;

    public override void _Ready()
    {
        _hint = GetNode<Control>("Hint");
        _hint.Visible = false;
    }

    public void ShowHint(bool isOpen)
    {
        _hint.Visible = isOpen;
    }
}
