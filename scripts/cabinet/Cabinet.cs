using Godot;
using HanaCoz.Helpers.Models;
using HanaCoz.Helpers.Signal;

namespace HanaCoz.Scripts.Cabinet;

public partial class Cabinet : StaticBody2D
{
    [Export] public string Code { get; set; }
    private AnimatedSprite2D _anim;
    private bool IsOpen { get; set; }
    private bool HaveItem { get; set; } = true;

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    private void _on_area_2d_body_entered(Node2D body)
    {
        if (body is not Player.Player player) return; 
        SignalBus.Instance.Emit(SignalNames.CabinetName.IsNear, this, player, true);
        SignalBus.Instance.Emit(SignalNames.UiName.InteractiveHintOpenClose, true);
    }
    private void _on_area_2d_body_exited(Node2D body)
    {
        if (body is not Player.Player player) return; 
        SignalBus.Instance.Emit(SignalNames.CabinetName.IsNear, this, player, false);
        SignalBus.Instance.Emit(SignalNames.UiName.InteractiveHintOpenClose, false);
    }

    public void OnOpen(StorageEntity entity)
    {
        IsOpen = true;
        HaveItem = entity.Items.Count > 0;
        _anim.Play(HaveItem ? "openfull" : "openempty");
        
    }
    
    public void OnClose()
    {
        IsOpen = false;
        _anim.Play(HaveItem ? "closefull" : "closeempty");
    }

    public bool CheckOpen() => IsOpen;
}