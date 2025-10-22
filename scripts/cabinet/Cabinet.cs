using Godot;
using HanaCoz.Helpers;
using HanaCoz.Helpers.Models;
using HanaCoz.Helpers.Signal;
using HanaCoz.Scripts.Storage;

namespace HanaCoz.Scripts.Cabinet;

public partial class Cabinet : StaticBody2D
{
    [Export] public string Code { get; set; }
    private AnimatedSprite2D _anim;
    public bool IsOpen { get; set; }
    private bool HaveItem { get; set; } = true;
    private StorageEntity _data;
    public StorageEntity Data
    {
        get => _data;
        set
        {
            _data = value;
            HaveItem = value.Items.Count > 0;
            if (!IsOpen) OnOpen();
            else OnClose();
        }
    }

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    
    private void _on_area_2d_body_entered(Node2D body)
    {
        if (body is not Player.Player player) return; 
        SignalBus.Instance.Emit(SignalNames.Behavior.PlayerIsNearInteractiveZone, true, this);
    }
    private void _on_area_2d_body_exited(Node2D body)
    {
        if (body is not Player.Player player) return; 
        SignalBus.Instance.Emit(SignalNames.Behavior.PlayerIsNearInteractiveZone, false, this);
    }

    private void OnOpen()
    {
        IsOpen = true;
        _anim.Play(HaveItem ? "openfull" : "openempty");
        
    }
    
    private void OnClose()
    {
        IsOpen = false;
        _anim.Play(HaveItem ? "closefull" : "closeempty");
    }
}