using Godot;
using HanaCoz.Helpers.Models;
using HanaCoz.Scripts.Player;

namespace HanaCoz;

public partial class MainPlayerMenu: Node
{
    public Player Load(PlayerEntity pdata)
    {
        var playerScene = GD.Load<PackedScene>("res://scenes/player.tscn");
        var player = playerScene.Instantiate<Player>();
        player.Data = pdata;
        return player;
    }
}