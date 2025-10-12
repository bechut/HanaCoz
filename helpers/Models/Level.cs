#nullable enable

namespace HanaCoz.Helpers.Models;

public class LevelEntity
{
    public int Id { get; set; }
    public int No { get; init; } = 1;
    public PlayerEntity? Player { get; set; }
}