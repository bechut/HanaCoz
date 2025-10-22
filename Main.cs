using Godot;
using HanaCoz.Helpers;
using HanaCoz.Helpers.Models;
using HanaCoz.Helpers.Signal;
using HanaCoz.Scripts.Cabinet;
using HanaCoz.Scripts.Panel;
using HanaCoz.Scripts.Player;
using HanaCoz.Scripts.Storage;
using HanaCoz.Scripts.UI;

namespace HanaCoz;

public partial class Main : Node2D
{
    private double _autoSaveCounting;
    private PlayerMenu _playerMenu;
    private StorageUi _storageUi;
    private InteractiveUi _interactiveUi;
    private Player _player;
    private StorageSlotTexture _slotTexture;
    private Node _currentPanel;
    
    private MainEntity _mainData;
    
    public override void _Ready()
    {
        _mainData = Global.MainService.GetData();

        _playerMenu = GetNode<PlayerMenu>("UI/PlayerMenu");
        _storageUi =  GetNode<StorageUi>("UI/StorageUI");
        _interactiveUi = GetNode<InteractiveUi>("UI/InteractiveUI");

        if (_mainData is { Level.Player: not null })
        {
            _player?.QueueFree();
            var playerScene = GD.Load<PackedScene>("res://scenes/player.tscn");
            _player = playerScene.Instantiate<Player>();
            AddChild(_player);
            _player.Data = _mainData.Level.Player;
        }
        
        SignalBus.Instance.On(SignalNames.Action.PlayerOpenCloseCabinet, this, PlayerOpenCloseCabinet);
        SignalBus.Instance.On(SignalNames.Action.StorageItemDragAndDrop, this, StorageItemDragAndDrop);
        SignalBus.Instance.On(SignalNames.Action.PlayerEquipItemFromStorage, this, PlayerEquipItemFromStorage);
        SignalBus.Instance.On(SignalNames.Action.OpenPlayerMenu, this, OpenPlayerMenu);
    }
    
    private void PlayerOpenCloseCabinet(Variant[] args)
    {
        var isOpen = args[0].AsBool();
        if (args[1].AsGodotObject() is not Cabinet cabinet) return;
        if (isOpen)
        {
            AutoSave();
            var data = Global.StorageService.GetStorageByCode(cabinet.Code);
            cabinet.Data = data;
            _storageUi.Data = data;
        }
        else
        {
            cabinet.Data = cabinet?.Data;
            _storageUi.Data = null;
        }
    }
    
    private void StorageItemDragAndDrop(Variant[] args)
    {
        var target = args[0].AsGodotObject() as StorageSlotTexture;
        var source = args[1].AsGodotObject() as StorageSlotTexture;
    
        if (target != null && source != null)
        {
            var targetMeta = target.GetMeta("meta").AsGodotDictionary();
            var sourceMeta= source.GetMeta("meta").AsGodotDictionary();
            Global.StorageService.UpdateItemOrder(sourceMeta["ItemId"].AsInt32(), targetMeta["ItemOrder"].AsInt32());
            if (targetMeta.ContainsKey("ItemId") && targetMeta["ItemId"].AsInt32() > 0)
            {
                Global.StorageService.UpdateItemOrder(targetMeta["ItemId"].AsInt32(), sourceMeta["ItemOrder"].AsInt32());
            }
            SignalBus.Instance.Emit(SignalNames.Action.PlayerOpenCloseCabinet, true, _player.Cabinet);
        }
    }

    private void PlayerEquipItemFromStorage(Variant[] args)
    {
        var meta = args[0].AsGodotDictionary();
        if (_mainData is { Level.Player: not null })
        {
            var itemType = meta["Type"].ToString();
            
            var playerItem = _player.Data.Items.Find(item => item.Type == itemType);
            if (playerItem.Item == null) return;
            
            Global.PlayerService.UpdatePlayerItemItem(playerItem.Id, meta["ItemItemId"].AsInt32());
            Global.StorageService.UpdateStorageItemItem(meta["ItemId"].AsInt32(), playerItem.Item.Id);
            
            var updatedStorage = Global.StorageService.GetStorageByCode(_player.Cabinet.Code);
            _storageUi.Data = updatedStorage;
            var updatedPlayer = Global.PlayerService.GetMainPlayer();
            _player.Data = updatedPlayer;
            if (_currentPanel is PlayerPanel p)
            {
                p.Data = _player.Data.Items;
            }
        }
    }
    
    private void OpenPlayerMenu(Variant[] args)
    {
        var panel = args[0].AsGodotObject() as Node;
        _currentPanel = panel;
        if (_player.Data != null && _currentPanel is PlayerPanel p)
        {
            p.Data = _player.Data.Items;
        }
    }

    private void AutoSave()
    {
        Global.PlayerService.UpdatePlayer(_player.Data.Id, _player.Position.X, _player.Position.Y);
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            _slotTexture?.CloseContextMenu();
        }
    }

    public override void _Process(double delta)
    {
        _autoSaveCounting += delta;
        if (_autoSaveCounting >= 5)
        {
            GD.Print("saved");
            AutoSave();
            _autoSaveCounting = 0;
        }
    }
}
