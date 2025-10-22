using System.Collections.Generic;
using System.Linq;
using Godot;
using HanaCoz.Helpers.Models;
using HanaCoz.Scripts.Storage;

namespace HanaCoz.Scripts.Panel;
public partial class PlayerPanel : Control
{
    private Control _outfit;
    private Control _hair;
    private PackedScene _texture;
    
    private List<PlayerItemEntity> _data;
    public List<PlayerItemEntity> Data
    {
        get => _data;
        set
        {
            _data = value;
            LoadTexture();
        }
    }

    public override void _Ready()
    {   
        _outfit = GetNode<Control>("GridR/Outfit");
        _hair =  GetNode<Control>("GridR/Hair");
            
        // _texture = GD.Load<PackedScene>("res://scenes/panel_slot_texture.tscn");
        // if (_texture == null)
        //     GD.PrintErr("Failed to load panel_slot_texture!");
    }

    public void LoadTexture()
    {
        for (int i = 0; i < _data.Count; i++)
        {
            var item = _data[i];
            var meta = new Godot.Collections.Dictionary();
            meta.Add("ItemOrder", i);
            meta.Add("From", item.Type);
            meta.Add("ItemId", item.Id);
            meta.Add("Type", item.Type);

            var node = item.Type switch
            {
                "outfit" => _outfit,
                "hairstyle" => _hair,
                _ => null
            };

            var rect = node?.GetNodeOrNull<TextureRect>("SlotTexture");
            if (rect == null) return;
            rect.SetMeta("meta", meta);
            var texturePath = $"res://assets/{item.Type}/{item.Item.Code}.png";
            var texture = GD.Load<Texture2D>(texturePath);
            rect.Texture = new AtlasTexture
            {
                Atlas = texture,
                Region = new Rect2(item.Item.RegionX, item.Item.RegionY, item.Item.RegionW, item.Item.RegionH)
            };
        }
    }
}
