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
    public AnimatedSprite2D Anim;
    public AnimatedSprite2D Outfit;
    public AnimatedSprite2D Hair;
    public string LastDir { get; private set; } = "d";
    public bool IsSit { get; set; }
    public bool CanInteract { get; set; }
    public bool CanMove { get; set; } = true;

    private PlayerEntity _data;
    public PlayerEntity Data
    {
        get => _data;
        set
        {
            _data = value;
            OnChangePlayerAsset();
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
        GlobalPosition = new Vector2(Data.PosX, Data.PosY);
        
        Anim =  GetNode<AnimatedSprite2D>("Anim");
        Outfit = GetNode<AnimatedSprite2D>("Outfit");
        Hair = GetNode<AnimatedSprite2D>("Hair");

        _fsm = new StateMachine<Player>(this);
        States[PlayerStateType.Idle] = new IdleState();
        States[PlayerStateType.Run]  = new RunState();
        States[PlayerStateType.Sit]  = new SitState();
        
        _fsm.ChangeState(States[PlayerStateType.Idle]);
        
        OnChangePlayerAsset();
        
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (CanMove)
        {
            var vel = Helper.InputVector();
            Velocity = vel * Data.Speed;
            LastDir = Helper.MoveDirection(Velocity, LastDir);
            MoveAndSlide();
            _fsm.PhysicsUpdate(delta);
        }
        if (Input.IsActionJustPressed("ui_interact"))
        {
            if (CanInteract)
            {
                SignalBus.Instance.Emit(SignalNames.MainName.FetchStorageData);
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
