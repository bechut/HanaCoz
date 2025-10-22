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
    private Cabinet _cabinet;
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
            _player = MainPlayer.Load(_mainData.Level.Player);
            AddChild(_player);
        }
        
        // SignalBus.Instance.On(SignalNames.UiName.CurrentPlayerPanel, this, OnReceiveCurrentPlayerPanel);
        // SignalBus.Instance.On(SignalNames.UiName.CurrentSlotTexture,this, OnCurrentSlotTexture);
        // SignalBus.Instance.On(SignalNames.CabinetName.IsNear, this, OnIsNearCabinet);
        // SignalBus.Instance.On(SignalNames.UiName.InteractiveHintOpenClose, this, OnInteractiveHintOpenClose);
        // SignalBus.Instance.On(SignalNames.MainName.FetchStorageData, this, OnFetchStorageData);
        // SignalBus.Instance.On(SignalNames.MainName.UpdateStorageItemOrder, this, OnUpdateStorageItemOrder);
        // SignalBus.Instance.On(SignalNames.MainName.CloseStorage, this, OnCloseStorage);
        // SignalBus.Instance.On(SignalNames.MainName.EquipItem, this, OnEquipItem);
        
        SignalBus.Instance.On(SignalNames.Action.PlayerOpenCloseCabinet, this, PlayerOpenCloseCabinet);
    }
    
    private void PlayerOpenCloseCabinet(Variant[] args)
    {
        var isOpen = args[0].AsBool();
        if (args[1].AsGodotObject() is not Cabinet cabinet) return;
        if (isOpen)
        {
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

    // private void OnEquipItem(Variant[] args)
    // {
    //     var meta = args[0].AsGodotDictionary();
    //     if (_mainData is { Level.Player: not null })
    //     {
    //         var itemType = meta["Type"].ToString();
    //         
    //         var playerItem = _player.Data.Items.Find(item => item.Type == itemType);
    //         if (playerItem.Item == null) return;
    //         
    //         Global.PlayerService.UpdatePlayerItemItem(playerItem.Id, meta["ItemItemId"].AsInt32());
    //         Global.StorageService.UpdateStorageItemItem(meta["ItemId"].AsInt32(), playerItem.Item.Id);
    //         
    //         var updatedStorage = Global.StorageService.GetStorageByCode(_cabinet.Code);
    //         _storageUi.Data = updatedStorage;
    //         var updatedPlayer = Global.PlayerService.GetMainPlayer();
    //         _player.Data = updatedPlayer;
    //         if (_currentPanel is PlayerPanel p)
    //         {
    //             p.LoadTexture(_player.Data.Items);
    //         }
    //     }
    // }
        
    // private void OnInteractiveHintOpenClose(Variant[] args)
    // {
    //     var isOpen = args[0].AsBool();
    //     _interactiveUi.ShowHint(isOpen);
    // }
    
    // private void OnIsNearCabinet(Variant[] args)
    // {
    //     var cabinet = args[0].AsGodotObject() as Cabinet;
    //     var player = args[1].AsGodotObject() as Player;
    //     var isNear = args[2].AsBool();
    //     if (cabinet != null && player != null)
    //     {
    //         _cabinet = cabinet;
    //         _player = player;
    //         _player.CanInteract = isNear;
    //     };
    // }
    
    // private void OnCloseStorage(Variant[] args)
    // {
    //     // _cabinet.OnClose();
    //     _storageUi.OnOpenClose(false);
    //     _player.CanMove = true;
    //     _slotTexture?.CloseContextMenu();
    //
    // }

    // private void OnFetchStorageData(Variant[] args)
    // {
    //     if (_cabinet == null) return;
    //     if (_cabinet.CheckOpen())
    //     {
    //         OnCloseStorage(args);
    //     }
    //     else
    //     {
    //         var data = Global.StorageService.GetStorageByCode(_cabinet.Code);
    //         _cabinet.OnOpen(data);
    //         _storageUi.OnOpenClose(true);
    //         _storageUi.Data = data;
    //         _player.CanMove = false;
    //     }
    // }
    
    // private void OnUpdateStorageItemOrder(Variant[] args)
    // {
    //     var target = args[0].AsGodotObject() as StorageSlotTexture;
    //     var source = args[1].AsGodotObject() as StorageSlotTexture;
    //
    //     if (target != null && source != null)
    //     {
    //         var targetMeta = target.GetMeta("meta").AsGodotDictionary();
    //         var sourceMeta= source.GetMeta("meta").AsGodotDictionary();
    //         Global.StorageService.UpdateItemOrder(sourceMeta["ItemId"].AsInt32(), targetMeta["ItemOrder"].AsInt32());
    //         if (targetMeta.ContainsKey("ItemId") && targetMeta["ItemId"].AsInt32() > 0)
    //         {
    //             Global.StorageService.UpdateItemOrder(targetMeta["ItemId"].AsInt32(), sourceMeta["ItemOrder"].AsInt32());
    //         }
    //         var data = Global.StorageService.GetStorageByCode(_cabinet.Code);
    //         _storageUi.Data = data;
    //     }
    // }
    //
    // private void OnReceiveCurrentPlayerPanel(Variant[] args)
    // {
    //     var panel = args[0].AsGodotObject() as Node;
    //     _currentPanel = panel;
    //     if (_player.Data != null && _currentPanel is PlayerPanel p)
    //     {
    //         p.LoadTexture(_player.Data.Items);
    //     }
    // }
    //
    // private void OnCurrentSlotTexture(Variant[] args)
    // {
    //     _slotTexture = args[0].AsGodotObject() as StorageSlotTexture;
    // }

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
            _autoSaveCounting = 0;
        }
    }
}
