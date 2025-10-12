using Godot;
using HanaCoz.Helpers.Signal;

namespace HanaCoz.Scripts.Storage;

public partial class StorageSlotTexture : TextureRect
{
    private Color _defaultColor =  new (1,1,1);
    private Color _hoverColor =  new (0.7f, 0.85f, 1);
    
    private TextureRect _slot;
    private NinePatchRect _contextMenu; 
    private TextureButton _equipButton; 
    private static NinePatchRect _activeContextMenu;
    
    public override void _Ready()
    {
        _slot = GetParent().GetNode<TextureRect>("Slot");
        _contextMenu = GetNode<NinePatchRect>("../ContextMenu");
        _equipButton = _contextMenu.GetNode<TextureButton>("TextureButton");
        _contextMenu.Hide();
        _equipButton.Pressed += OnEquip;
    }

    private void OnEquip()
    {
        if (HasMeta("meta"))
        {
            var meta = GetMeta("meta").AsGodotDictionary();
            GD.Print(meta);
            if (meta["From"].ToString() is "storage")
            {
                SignalBus.Instance.Emit(SignalNames.MainName.EquipItem, meta);
            }
        }
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        var preview = new TextureRect
        {
            Texture = this.Texture,
            StretchMode = StretchModeEnum.KeepAspectCentered,
            Scale = new Vector2(1.5f, 1.5f)
        };
        SetDragPreview(preview);
        return Variant.CreateFrom(this.GetPath());
    }
    
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        var source = GetNode<StorageSlotTexture>(data.AsString());
        if (HasMeta("meta"))
        {
            _slot.Modulate = _hoverColor;
            var meta = (Godot.Collections.Dictionary)GetMeta("meta");
            var sourceMeta = (Godot.Collections.Dictionary)source.GetMeta("meta");
            return meta["From"].ToString() == sourceMeta["From"].ToString();
        }
        return false;
    }
    
    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var source = GetNode<StorageSlotTexture>(data.AsString());
        if (HasMeta("meta"))
        {
            var meta = (Godot.Collections.Dictionary)GetMeta("meta");
            var sourceMeta = (Godot.Collections.Dictionary)source.GetMeta("meta");
            
            if (meta["ItemOrder"].ToString() == sourceMeta["ItemOrder"].ToString()) return;
            if (sourceMeta["ItemOrder"].AsInt32() > -1)
            {
                SignalBus.Instance.Emit(SignalNames.MainName.UpdateStorageItemOrder, this, source);
            }
            
        }
    }
    
    public override void _Notification(int what)
    {
        if (what == NotificationMouseExit)
        {
            _slot.Modulate = _defaultColor;
        }
    }
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Right, Pressed: true })
        {
            if (HasMeta("meta"))
            {
                var meta = this.GetMeta("meta").AsGodotDictionary();
                if (!meta.ContainsKey("ItemId")) return;
                GD.Print(this.GetMeta("meta"));
                if (_activeContextMenu != null && _activeContextMenu != _contextMenu)
                {
                    _activeContextMenu.Hide();
                }
                _contextMenu.Show();
                _contextMenu.Position = GlobalPosition + Size;
                _activeContextMenu = _contextMenu;
                SignalBus.Instance.Emit(SignalNames.UiName.CurrentSlotTexture, this);
            }
        }
    }

    public void CloseContextMenu()
    { 
        _contextMenu.Hide();
        _activeContextMenu = null;
    }
}
