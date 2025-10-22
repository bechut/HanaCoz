using Godot;
using Godot.Collections;

namespace HanaCoz.Helpers.Signal;

public static class EventHelper
{
    private static Node _main; // cache lại

    private static Node GetMain(Node sender)
    {
        if (_main == null || !_main.IsInsideTree())
        {
            _main = sender.GetTree().Root.GetNodeOrNull<Node>("Main");
        }

        return _main;
    }

    public static void Send(Node sender, string targetPath, string message, Variant custom)
    {
        var main = GetMain(sender);
        if (main == null)
        {
            GD.PrintErr("[EventHelper] Main not found");
            return;
        }

        main.Call("ProcessEvent", sender.GetPath(), targetPath, message, custom);
    }
    
    // public void ProcessEvent(string fromPath, string targetPath, string message, Variant custom)
    // {
    //     GD.Print($"[Main] Event from {fromPath} -> {targetPath}: {message} - {custom}");
    //
    //     Node target;
    //
    //     // --- Nếu gửi cho chính Main ---
    //     if (targetPath == "." || targetPath == GetPath())
    //     {
    //         target = this;
    //     }
    //     else if (!_nodeCache.TryGetValue(targetPath, out target) || target == null || !IsInstanceValid(target))
    //     {
    //         target = GetNodeOrNull<Node>(targetPath);
    //         if (target == null)
    //         {
    //             GD.PrintErr($"[Main] Target '{targetPath}' not found");
    //             return;
    //         }
    //
    //         _nodeCache[targetPath] = target; // Cache it
    //     }
    //
    //     // --- Gọi handler ---
    //     if (target.HasMethod("OnEventReceived"))
    //         target.Call("OnEventReceived", fromPath, message, custom);
    //     else
    //         GD.PrintErr($"[Main] Target '{targetPath}' has no OnEventReceived method");
    // }
}
