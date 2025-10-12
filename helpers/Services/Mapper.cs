using System.Data;
using HanaCoz.Helpers.Models;
using Microsoft.Data.Sqlite;

namespace HanaCoz.Helpers.Services;

public static class DataMapper
{
  public static ItemEntity ItemEntityMapper(SqliteDataReader reader)
  {
    return new ItemEntity
    {
      Id = reader.GetInt32(reader.GetOrdinal("item_id")),
      Code = reader.GetString(reader.GetOrdinal("code")),
      Name = reader.GetString(reader.GetOrdinal("name")),
      Description = reader.GetString(reader.GetOrdinal("description")),
      Type = reader.GetString(reader.GetOrdinal("type")),
      RegionX = reader.GetInt32(reader.GetOrdinal("region_x")),
      RegionY = reader.GetInt32(reader.GetOrdinal("region_y")),
      RegionW = reader.GetInt32(reader.GetOrdinal("region_w")),
      RegionH = reader.GetInt32(reader.GetOrdinal("region_h")),
    };
  }

  public static StorageItemEntity StorageItemEntityMapper(SqliteDataReader reader)
  {
    return new StorageItemEntity
    {
      Id = reader.GetInt32(reader.GetOrdinal("storage_item_id")),
      ItemOrder = reader.GetInt32(reader.GetOrdinal("item_order")),
      Item = ItemEntityMapper(reader)
    };
  }
  
  public static StorageEntity StorageEntityMapper(SqliteDataReader reader)
  {
    return new StorageEntity
    {
      Id = reader.GetInt32(reader.GetOrdinal("storage_id")),
      Code = reader.GetString(reader.GetOrdinal("storage_code")),
      Items = []
    };
  }
  
  public static PlayerEntity PlayerEntityMapper(SqliteDataReader reader)
  {
    return new PlayerEntity
    {
      Id = reader.GetInt32(reader.GetOrdinal("player_id")),
      Code = reader.GetString(reader.GetOrdinal("player_code")),
      PosX = reader.GetInt32(reader.GetOrdinal("pos_x")),
      PosY = reader.GetInt32(reader.GetOrdinal("pos_y")),
      Speed = reader.GetInt32(reader.GetOrdinal("run_speed")),
      Items = []
    };
  }
  
  public static PlayerItemEntity PlayerItemEntityMapper(SqliteDataReader reader)
  {
    return new PlayerItemEntity
    {
      Id = reader.GetInt32(reader.GetOrdinal("player_item_id")),
      Equipped = reader.GetInt32(reader.GetOrdinal("equipped")),
      Type = reader.GetString(reader.GetOrdinal("type")),
      Item = ItemEntityMapper(reader)
    };
  }
  
  public static MainEntity MainEntityMapper(SqliteDataReader reader)
  {
    return new MainEntity
    {
      Id = reader.GetInt32(reader.GetOrdinal("player_item_id")),
      Level = LevelEntityMapper(reader),
    };
  }
  
  public static LevelEntity LevelEntityMapper(SqliteDataReader reader)
  {
    return new LevelEntity
    {
      Id = reader.GetInt32(reader.GetOrdinal("player_item_id")),
      No = reader.GetInt32(reader.GetOrdinal("level_no")),
      Player = PlayerEntityMapper(reader)
    };
  }
}