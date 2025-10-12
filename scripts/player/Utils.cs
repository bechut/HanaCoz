using Godot;
using HanaCoz.Helpers;
using HanaCoz.Helpers.Models;
using HanaCoz.Helpers.Player;

namespace HanaCoz.Scripts.Player;

public static class PlayerUtils
{
    public static void ApplyEquippedAnim(string type, AnimatedSprite2D sprite, PlayerEntity data)
    {
        var equipped = data.Items.Find(item => item is { Equipped: 1, Type: var t } && t == type);
        if (equipped?.Item is null) return;

        var assetPath = $"{Global.AssetPath}/{type}/{equipped.Item.Code}.png";
        Helper.ChangeAnim(sprite, assetPath);
    }
}