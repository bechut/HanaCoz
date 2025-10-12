namespace HanaCoz.Helpers.Signal;

public static class SignalNames
{
    public static class CabinetName
    {
        public const string IsNear = "Cabinet.IsNear";
    }
    
    public static class MainName
    {
        public const string FetchStorageData = "Main.FetchStorageData";
        public const string UpdateStorageItemOrder = "Main.UpdateStorageItemOrder";
        public const string CloseStorage = "Main.CloseStorage";
        public const string EquipItem = "Main.EquipItem";
    }
    
    public static class UiName
    {
        public const string InteractiveHintOpenClose = "InteractiveHint.OpenClose";
        public const string CurrentPlayerPanel = "PlayerPanel.Current";
        public const string ContextMenuClose = "ContextMenu.Close";
        public const string CurrentSlotTexture = "SlotTexture.Current";
    }
}