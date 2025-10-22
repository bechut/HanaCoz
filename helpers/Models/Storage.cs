using System.Collections.Generic;

namespace HanaCoz.Helpers.Models;

public class StorageEntity
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public List<StorageItemEntity> Items { get; init; } = [];
}

public class StorageItemEntity
{
    public int Id { get; init; }
    public int ItemOrder { get; init; }
    public ItemEntity Item { get; init; }
}