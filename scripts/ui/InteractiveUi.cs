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
        
        SignalBus.Instance.On(SignalNames.Behavior.PlayerIsNearInteractiveZone, this, PlayerIsNearInteractiveZone);
    }
    
    private void PlayerIsNearInteractiveZone(Variant[] args)
    {
        var isNear = args[0].AsBool();
        _hint.Visible = isNear;
    }
}
