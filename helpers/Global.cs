using HanaCoz.Helpers.Services;

namespace HanaCoz.Helpers;

public static class Global
{
    public static StorageService StorageService => DatabaseManagement.Instance.GetStorageService();
    public static PlayerService PlayerService => DatabaseManagement.Instance.GetPlayerService();
    public static LevelService LevelService => DatabaseManagement.Instance.GetLevelService();
    public static MainService MainService => DatabaseManagement.Instance.GetMainService();
    public static ItemService ItemService => DatabaseManagement.Instance.GetItemService();
    public static string AssetPath { get; } = "res://assets";
    public static string DbPath { get; } = "/root/DatabaseManagement";
}