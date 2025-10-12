using System.Threading.Tasks;
using HanaCoz.Helpers.Models;
using Microsoft.Data.Sqlite;

namespace HanaCoz.Helpers.Services;

public class StorageService(SqliteConnection connection)
{
    public StorageEntity GetStorageByCode(string code)
    {
        const string query = """
             SELECT
                 st.id AS storage_id,
                 si.id AS storage_item_id,
                 i.id as item_id,
                 st.code AS storage_code,
                 si.item_order,
                 i.*
             FROM storage AS st
                      INNER JOIN storage_item AS si ON si.storage_id = st.id
                      INNER JOIN item AS i ON si.item_id = i.id
             WHERE st.code = @code;
             """;

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@code", code);

        StorageEntity storage = null;
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            storage ??= DataMapper.StorageEntityMapper(reader);
            storage.Items.Add(DataMapper.StorageItemEntityMapper(reader));
        }
        return storage;
    }
    
    public void UpdateItemOrder(int itemStorageId, int itemOrder)
    {
        const string query = "UPDATE storage_item SET item_order = @item_order WHERE id = @id;";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", itemStorageId);
        command.Parameters.AddWithValue("@item_order", itemOrder);
        command.ExecuteNonQuery();
    }
    
    public void UpdateStorageItemItem(int storageItemId, int itemId)
    {
        const string query = "UPDATE storage_item SET item_id = @item_id WHERE id = @id;";
    
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", storageItemId);
        command.Parameters.AddWithValue("@item_id", itemId);
        command.ExecuteNonQuery();
    }

    public async Task Init()
    {
        const string query = """
            INSERT INTO storage (id, code) 
            SELECT 1, '001' 
            WHERE NOT EXISTS (SELECT 1 FROM storage);

            INSERT INTO storage_item (id, item_order, item_id, storage_id) 
            SELECT 1, 1, 1, 1
            WHERE NOT EXISTS (SELECT 1 FROM storage_item WHERE id = 1);

            INSERT INTO storage_item (id, item_order, item_id, storage_id) 
            SELECT 2, 2, 4, 1
            WHERE NOT EXISTS (SELECT 1 FROM storage_item WHERE id = 2)
         """;
        
    
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }
}