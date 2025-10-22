using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace HanaCoz.Helpers.Services;

public class LevelService(SqliteConnection connection)
{
    // public List<StorageItemEntity> GetAllStorages()
    // {
    //     var items = new List<StorageItemEntity>();
    //
    //     const string query = "SELECT id, storage_code, item_id FROM storage_item";
    //
    //     using var command = connection.CreateCommand();
    //     command.CommandText = query;
    //
    //     using var reader = command.ExecuteReader();
    //     while (reader.Read())
    //     {
    //         var item = new StorageItemEntity
    //         {
    //             Id = reader.GetInt32(0),
    //             Code = reader.GetString(1),
    //             ItemId = reader.IsDBNull(2) ? "" : reader.GetString(2),
    //             StorageCode = reader.IsDBNull(3) ? "" : reader.GetString(3)
    //         };
    //         items.Add(item);
    //     }
    //
    //     return items;
    // }

    // public StorageItemEntity[] GetStorageItemByCode(string code)
    // {
    //     const string query = """
    //          SELECT 
    //             T1.id, T1.storage_code, T1.item_order, T2.name, T2.description, T2.code, T2.type,
    //             T2.region_x,T2.region_y,T2.region_w, T2.region_h
    //          FROM 
    //             storage_item AS T1
    //          INNER JOIN
    //             item AS T2 ON T1.item_id = T2.id
    //          WHERE 
    //              T1.storage_code = @code;
    //          """;
    //     var results = new List<StorageItemEntity>();
    //
    //     using var command = connection.CreateCommand();
    //     command.CommandText = query;
    //     command.Parameters.AddWithValue("@code", code);
    //     
    //     using var reader = command.ExecuteReader();
    //     while (reader.Read())
    //     {
    //         var entity = new StorageItemEntity
    //         {
    //             Id = reader.GetInt32(reader.GetOrdinal("id")),
    //             Code = reader.GetString(reader.GetOrdinal("code")),
    //             Name = reader.GetString(reader.GetOrdinal("name")),
    //             Description = reader.GetString(reader.GetOrdinal("description")),
    //             Type = reader.GetString(reader.GetOrdinal("type")),
    //             RegionX = reader.GetInt32(reader.GetOrdinal("region_x")),
    //             RegionY = reader.GetInt32(reader.GetOrdinal("region_y")),
    //             RegionW = reader.GetInt32(reader.GetOrdinal("region_w")),
    //             RegionH = reader.GetInt32(reader.GetOrdinal("region_h")),
    //             ItemOrder = reader.GetInt32(reader.GetOrdinal("item_order")),
    //         };
    //
    //         results.Add(entity);
    //     }
    //
    //     return results.ToArray();
    // }
    
    // public void GetPlayerByCode(string code)
    // {
    //     const string query = "UPDATE storage_item SET item_order = @item_order WHERE id = @id;";
    //
    //     using var command = connection.CreateCommand();
    //     command.CommandText = query;
    //     command.Parameters.AddWithValue("@id", code);
    //     command.ExecuteNonQuery();
    // }

    public async Task Init()
    {
        const string query = """
                                INSERT INTO level (id, no, player_id) 
                                SELECT 1, 1, 1
                                WHERE NOT EXISTS (SELECT 1 FROM level)
                             """;
    
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }
}