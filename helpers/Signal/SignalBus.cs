using Godot;
using System;
using System.Collections.Generic;

namespace HanaCoz.Helpers.Signal;

public partial class SignalBus : Node
{
    public static SignalBus Instance { get; private set; }

    // Lưu danh sách listener theo tên sự kiện
    private readonly Dictionary<string, List<Action<Variant[]>>> _listeners = new();
    private readonly Dictionary<Node, List<(string eventName, Action<Variant[]> callback)>> _ownerMap = new();

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            GD.Print("✅ SignalBus initialized");
        }
        else
        {
            GD.PrintErr("⚠ SignalBus already exists — destroying duplicate");
            QueueFree();
        }
    }

    /// <summary>
    /// Lắng nghe một event với tên cụ thể
    /// </summary>
    public void On(string eventName, Node owner, Action<Variant[]> callback)
    {
        if (!_listeners.ContainsKey(eventName))
            _listeners[eventName] = new List<Action<Variant[]>>();

        _listeners[eventName].Add(callback);
        
        if (!_ownerMap.ContainsKey(owner))
        {
            _ownerMap[owner] = new();
            owner.TreeExiting += () => OffByOwner(owner);
        }

        _ownerMap[owner].Add((eventName, callback));
    }
    
    private void OffByOwner(Node owner)
    {
        if (!_ownerMap.ContainsKey(owner)) return;

        foreach (var (eventName, callback) in _ownerMap[owner])
        {
            Off(eventName, callback);
        }

        _ownerMap.Remove(owner);
    }


    /// <summary>
    /// Ngừng lắng nghe event
    /// </summary>
    public void Off(string eventName, Action<Variant[]> callback)
    {
        if (_listeners.ContainsKey(eventName))
            _listeners[eventName].Remove(callback);
    }

    /// <summary>
    /// Phát (emit) một event kèm dữ liệu
    /// </summary>
    public void Emit(string eventName, params Variant[] args)
    {
        GD.PrintRich($"📡 [color=cyan]Emit:[/color] [b]{eventName}[/b] ({args.Length} arg(s))");

        if (args.Length > 0)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Variant arg = args[i];
                string typeName = arg.VariantType.ToString();
                string valueString = arg.ToString();

                // Nếu là Node, in cả path cho dễ theo dõi
                if (arg.AsGodotObject() is Node node)
                    valueString = $"[NodePath: {node.GetPath()}]";

                GD.PrintRich($"   ├─ [color=gray]Arg[{i}][/color] ({typeName}): [b]{valueString}[/b]");
            }
        }

        if (_listeners.TryGetValue(eventName, out var callbacks))
        {
            foreach (var cb in callbacks.ToArray())
            {
                try
                {
                    cb.Invoke(args);

                    string targetName = "(unknown)";
                    string targetClass = cb.Target?.GetType().Name ?? "null";

                    if (cb.Target is Node node)
                        targetName = node.Name;

                    GD.PrintRich($"👀 [color=blue]Delivered[/color] [b]{targetName}[/b] ({targetClass}) {cb.Method.Name}()");
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"❌ Error in listener for {eventName}: {ex.Message}");
                }
            }
        }
        else
        {
            GD.PrintRich($"⚠ [color=orange]No listeners for event:[/color] {eventName}");
        }
    }


}