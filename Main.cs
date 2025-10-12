using System;
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
    private PlayerMenu _playerMenu;
    private StorageUi _storageUi;
    private InteractiveUi _interactiveUi;
    private Player _player;
    private Cabinet _cabinet;
    private StorageSlotTexture _slotTexture;
    private MainEntity _data;
    private PlayerPanel _playerPanel;
    
    public override void _Ready()
    {
        _data = Global.MainService.GetData();

        _playerMenu = GetNode<PlayerMenu>("UI/PlayerMenu");
        _storageUi =  GetNode<StorageUi>("UI/StorageUI");
        _interactiveUi = GetNode<InteractiveUi>("UI/InteractiveUI");

        if (_data is { Level.Player: not null })
        {
            _player?.QueueFree();
            _player = MainPlayer.Load(_data.Level.Player);
            AddChild(_player);
        }
        
        SignalBus.Instance.On(SignalNames.UiName.CurrentPlayerPanel, this, OnReceiveCurrentPlayerPanel);
        SignalBus.Instance.On(SignalNames.UiName.CurrentSlotTexture,this, OnCurrentSlotTexture);
        SignalBus.Instance.On(SignalNames.CabinetName.IsNear, this, OnIsNearCabinet);
        SignalBus.Instance.On(SignalNames.UiName.InteractiveHintOpenClose, this, OnInteractiveHintOpenClose);
        SignalBus.Instance.On(SignalNames.MainName.FetchStorageData, this, OnFetchStorageData);
        SignalBus.Instance.On(SignalNames.MainName.UpdateStorageItemOrder, this, OnUpdateStorageItemOrder);
        SignalBus.Instance.On(SignalNames.MainName.CloseStorage, this, OnCloseStorage);
        SignalBus.Instance.On(SignalNames.MainName.EquipItem, this, OnEquipItem);
    }

    private void OnEquipItem(Variant[] args)
    {
        var meta = args[0].AsGodotDictionary();
        if (_data is { Level.Player: not null })
        {
            var itemType = meta["Type"].ToString();
            
            var playerItem = _player.Data.Items.Find(item => item.Type == itemType);
            if (playerItem.Item == null) return;
            
            Global.PlayerService.UpdatePlayerItemItem(playerItem.Id, meta["ItemItemId"].AsInt32());
            Global.StorageService.UpdateStorageItemItem(meta["ItemId"].AsInt32(), playerItem.Item.Id);
            var data = Global.StorageService.GetStorageByCode(_cabinet.Code);
            _storageUi.OnLoadData(data);
            var updatedPlayer = Global.PlayerService.GetMainPlayer();
            _player.Data = updatedPlayer;
            _player.OnChangePlayerAsset();
        }
    }
        
    private void OnInteractiveHintOpenClose(Variant[] args)
    {
        var isOpen = args[0].AsBool();
        _interactiveUi.ShowHint(isOpen);
    }
    
    private void OnIsNearCabinet(Variant[] args)
    {
        var cabinet = args[0].AsGodotObject() as Cabinet;
        var player = args[1].AsGodotObject() as Player;
        var isNear = args[2].AsBool();
        if (cabinet != null && player != null)
        {
            _cabinet = cabinet;
            _player = player;
            _player.CanInteract = isNear;
        };
        
    }
    
    private void OnCloseStorage(Variant[] args)
    {
        _cabinet.OnClose();
        _storageUi.OnOpenClose(false);
        _player.CanMove = true;
    }

    private void OnFetchStorageData(Variant[] args)
    {
        if (_cabinet == null) return;
        if (_cabinet.CheckOpen())
        {
            _cabinet.OnClose();
            _storageUi.OnOpenClose(false);
        }
        else
        {
            var data = Global.StorageService.GetStorageByCode(_cabinet.Code);
            _cabinet.OnOpen(data);
            _storageUi.OnOpenClose(true);
            _storageUi.OnLoadData(data);
        }
    }
    
    private void OnUpdateStorageItemOrder(Variant[] args)
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
            var data = Global.StorageService.GetStorageByCode(_cabinet.Code);
            _storageUi.OnLoadData(data);
        }
    }

    private void OnReceiveCurrentPlayerPanel(Variant[] args)
    {
        var panel = args[0].AsGodotObject() as PlayerPanel;
        if (_data is { Level.Player: not null })
        {
            panel?.LoadTexture(_data.Level.Player.Items);
        }
    }

    private void OnCurrentSlotTexture(Variant[] args)
    {
        _slotTexture = args[0].AsGodotObject() as StorageSlotTexture;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            _slotTexture?.CloseContextMenu();
        }
    }
}
