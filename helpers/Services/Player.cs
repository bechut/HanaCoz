using System.Collections.Generic;
using System.Threading.Tasks;
using HanaCoz.Helpers.Models;
using Microsoft.Data.Sqlite;

namespace HanaCoz.Helpers.Services;

public class PlayerService(SqliteConnection connection)
{
    public void UpdatePlayerItemItem(int playerItemId, int itemId)
    {
        const string query = "UPDATE player_item SET item_id = @item_id WHERE id = @id;";
    
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", playerItemId);
        command.Parameters.AddWithValue("@item_id", itemId);
        command.ExecuteNonQuery();
    }
    
    public PlayerEntity GetMainPlayer()
    {
        const string query = """
         SELECT
             p.id AS player_id,
             p.code AS player_code,
             p.pos_x,
             p.pos_y,
             p.run_speed,
             pi.id AS player_item_id,
             pi.item_id,
             pi.equipped,
             pi.type,
             i.type,
             i.code,
             i.name,
             i.region_x,
             i.region_y,
             i.region_w,
             i.region_h,
             i.description
         FROM player AS p
                  INNER JOIN player_item AS pi ON pi.player_id = p.id
                  INNER JOIN item AS i ON pi.item_id = i.id
         WHERE p.code = 'main';
         """;
    
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
        
        PlayerEntity player = null;
        
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            player ??= DataMapper.PlayerEntityMapper(reader);
            player.Items.Add(DataMapper.PlayerItemEntityMapper(reader));
        }

        return player;
    }

    public async Task Init()
    {
        const string query = """
            INSERT INTO player (id, pos_x, pos_y, code, run_speed) 
            SELECT 1, 100, 100, 'main' , 200
            WHERE NOT EXISTS (SELECT 1 FROM player)
         """;
    
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }
}