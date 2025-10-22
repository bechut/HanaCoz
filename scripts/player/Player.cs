using System;
using System.Collections.Generic;
using Godot;
using HanaCoz.Helpers.Models;
using HanaCoz.Helpers.Player;
using HanaCoz.Helpers.Signal;
using HanaCoz.Utils.FiniteStateMachine;

namespace HanaCoz.Scripts.Player;

public partial class Player : CharacterBody2D
{
    private bool _isReady;
    public AnimatedSprite2D Anim;
    public AnimatedSprite2D Outfit;
    public AnimatedSprite2D Hair;
    public string LastDir { get; private set; } = "d";
    public bool IsSit { get; set; }
    private bool CanInteract { get; set; }
    private bool CanMove { get; set; } = true;
    private float Speed { get; set; } = 200f;

    private PlayerEntity _data;
    public Cabinet.Cabinet Cabinet;
    public PlayerEntity Data
    {
        get => _data;
        set
        {
            if (!_isReady) return;
            _data = value;
            OnChangePlayerAsset();
            Position = new Vector2(_data.PosX, _data.PosY);
            Speed = _data.Speed;
        }
    }

    public enum PlayerStateType
    {
        Idle,
        Run,
        Sit,
    }
    
    private StateMachine<Player> _fsm;
    public readonly Dictionary<PlayerStateType, IState<Player>> States = new();

    public override void _Ready()
    {
        Anim =  GetNode<AnimatedSprite2D>("Anim");
        Outfit = GetNode<AnimatedSprite2D>("Outfit");
        Hair = GetNode<AnimatedSprite2D>("Hair");

        _fsm = new StateMachine<Player>(this);
        States[PlayerStateType.Idle] = new IdleState();
        States[PlayerStateType.Run]  = new RunState();
        States[PlayerStateType.Sit]  = new SitState();
        
        _fsm.ChangeState(States[PlayerStateType.Idle]);

        
        SignalBus.Instance.On(SignalNames.Behavior.PlayerIsNearInteractiveZone, this, PlayerIsNearInteractiveZone);

        _isReady = true;
    }

    private void PlayerIsNearInteractiveZone(Variant[] args)
    {
        var isNear = args[0].AsBool();
        CanInteract = isNear;
        if (args[1].AsGodotObject() is Cabinet.Cabinet cabinet)
        {
            Cabinet = cabinet;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CanMove)
        {
            var vel = Helper.InputVector();
            Velocity = vel * Speed;
            LastDir = Helper.MoveDirection(Velocity, LastDir);
            MoveAndSlide();
            _fsm.PhysicsUpdate(delta);
        }
        if (Input.IsActionJustPressed("ui_interact"))
        {
            if (CanInteract)
            {
                SignalBus.Instance.Emit(SignalNames.Action.PlayerOpenCloseCabinet, !Cabinet.IsOpen, Cabinet);
            }
        }
    }
    
    public void OnChangePlayerAsset()
    {
        try
        {
            Helper.ChangeAnim(Outfit, "");
            Helper.ChangeAnim(Hair, "");
        
            PlayerUtils.ApplyEquippedAnim("outfit", Outfit, _data);
            PlayerUtils.ApplyEquippedAnim("hairstyle", Hair, _data);
        }
        catch (Exception e)
        {
            GD.Print(e);
        }
        
    }
}
