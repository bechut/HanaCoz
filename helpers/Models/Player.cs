#nullable enable

using System.Collections.Generic;

namespace HanaCoz.Helpers.Models;

public class PlayerItemEntity
{
    public int Id { get; init; }
    public int Equipped { get; init; }
    public string Type { get; init; } = "";
    public ItemEntity? Item { get; set; }
}

public class PlayerEntity
{
    public int Id { get; set; }
    public string Code { get; init; } = "";
    public int PosX { get; init; }
    public int PosY { get; init; }
    public int Speed { get; init; }
    public List<PlayerItemEntity> Items { get; init; } = [];
}