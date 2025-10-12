using System.Text.Json;
using Godot;
using HanaCoz.Helpers;
using HanaCoz.Helpers.Models;
using HanaCoz.Helpers.Signal;

namespace HanaCoz.Scripts.Storage;
public partial class StorageUi : NinePatchRect
{
    private GridContainer _gridContainer;
    private TextureButton _closeBtn;
    public override void _Ready()
    {
        Visible = false;
        _gridContainer = GetNode<GridContainer>("GridContainer");
        _closeBtn = GetNode<TextureButton>("CloseBtn");

        _closeBtn.Pressed += OnCloseButtonPressed;
    }
    
    private void OnCloseButtonPressed()
    {
        SignalBus.Instance.Emit(SignalNames.MainName.CloseStorage);
    }

    public void OnLoadData(StorageEntity data)
    {
        for (var i = 0; i < _gridContainer.GetChildCount(); i++)
        {
            var child = _gridContainer.GetChild(i);
            var slotTexture = child.GetNodeOrNull<TextureRect>("SlotTexture");
            slotTexture.Texture = null;
            var meta = new Godot.Collections.Dictionary();
            meta.Add("ItemOrder", i);
            meta.Add("From", "storage");
            meta.Add("StorageCode", data.Code);
            var exists = data.Items.Exists(x => x.ItemOrder == i);
            if (exists)
            {
                var d = data.Items.Find(x => x.ItemOrder == i);
                meta.Add("ItemId", d.Id);
                meta.Add("ItemItemId", d.Item.Id);
                meta.Add("Type", d.Item.Type);
                
                var texturePath = $"res://assets/{d.Item.Type}/{d.Item.Code}.png";
                var texture = GD.Load<Texture2D>(texturePath);
                slotTexture.Texture = new AtlasTexture
                {
                    Atlas = texture,
                    Region = new Rect2(d.Item.RegionX, d.Item.RegionY, d.Item.RegionW, d.Item.RegionH)
                };
            }
            slotTexture.SetMeta("meta", meta);
        }
    }

    public void OnOpenClose(bool isOpen)
    {
        Visible = isOpen;
    }
    
    public override void _ExitTree()
    {
        _closeBtn.Pressed -= OnCloseButtonPressed;
    }
}
