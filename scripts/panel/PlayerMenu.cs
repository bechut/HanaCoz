using Godot;
using HanaCoz.Helpers.Signal;

namespace HanaCoz.Scripts.Panel;

public partial class PlayerMenu : NinePatchRect
{
    public enum PanelType
    {
        Player,
        Bag,
        Skill,
    }

    private PanelType _currentPanel = PanelType.Player;
    private Godot.Panel _panel;
    private NinePatchRect _playerBtn;
    private NinePatchRect _bagBtn;
    private NinePatchRect _skillBtn;
    private PackedScene _panelScene;

    public override void _Ready()
    {
        Visible = false;
        _panel = GetNode<Godot.Panel>("Panel");
        _playerBtn = GetNode<NinePatchRect>("PlayerButton");
        _bagBtn = GetNode<NinePatchRect>("BagButton");
        // _skillBtn =  GetNode<NinePatchRect>("SkillButton");

        _playerBtn.GuiInput += (InputEvent @event) => OnPlayerBtnGuiInput(@event, PanelType.Player);
        _bagBtn.GuiInput += (InputEvent @event) => OnPlayerBtnGuiInput(@event, PanelType.Bag);
        // _skillBtn.GuiInput += (InputEvent @event) => OnPlayerBtnGuiInput(@event, PanelType.Skill);
    }

    private void OnPlayerBtnGuiInput(InputEvent @event, PanelType panelType)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            if (_currentPanel == panelType)
                return; // same panel, do nothing

            _currentPanel = panelType;
            AddPackedScene(panelType);
        }
    }

    private void AddPackedScene(PanelType panelType)
    {
        _panelScene = panelType == PanelType.Player
            ? GD.Load<PackedScene>("res://scenes/player_panel.tscn")
            : _panelScene = panelType == PanelType.Bag
                ? GD.Load<PackedScene>("res://scenes/bag_panel.tscn")
                : GD.Load<PackedScene>("res://scenes/skill_panel.tscn");
        foreach (var child in _panel.GetChildren())
        {
            child.QueueFree();
        }

        var p = _panelScene.Instantiate();
        _panel.AddChild(p);
        SignalBus.Instance.Emit(SignalNames.UiName.CurrentPlayerPanel, p);
        
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_info"))
        {
            Visible = !Visible;
            if (_panel.GetChildCount() == 0)
            {
                AddPackedScene(PanelType.Player);
            }
        }
    }
    
}