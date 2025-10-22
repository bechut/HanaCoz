namespace HanaCoz.Helpers.Models;

public enum ItemType
{
    None,
    Outfit,
    Accessory,
    Hairstyle
}

public class ItemEntity
{
    public int Id { get; set; }
    public string Name { get; init; } = "";
    public string Code { get; init; } = "";
    public string Description { get; init; } = "";
    public string Type { get; init; }
    public int RegionX { get; init; }
    public int RegionY { get; init; }
    public int RegionW { get; init; }
    public int RegionH { get; init; }
}
