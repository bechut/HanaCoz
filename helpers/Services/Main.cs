using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Godot;
using HanaCoz.Helpers.Models;

namespace HanaCoz.Helpers.Services;

public class MainService(SqliteConnection connection)
{
    public MainEntity GetData()
    {
        const string query = """
             SELECT 
                 m.id AS main_id,
                 lv.no AS level_no,
                 p.id AS player_id,
                 p.code AS player_code,
                 p.pos_x,
                 p.pos_y,
                 p.run_speed,
                 pi.id AS player_item_id,
                 pi.item_id,
                 pi.equipped,
                 i.type,
                 i.code,
                 i.name,
                 i.region_x,
                 i.region_y,
                 i.region_w,
                 i.region_h,
                 i.description
             FROM main as m
             INNER JOIN level as lv ON m.level_id = lv.id
             INNER JOIN player as p ON lv.player_id = p.id
             INNER JOIN player_item AS pi ON pi.player_id = p.id
             INNER JOIN item AS i ON pi.item_id = i.id;
         """;

        using var command = connection.CreateCommand();
        command.CommandText = query;

        MainEntity main = null;

        try
        {
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                main ??= DataMapper.MainEntityMapper(reader);
                main.Level.Player.Items.Add(DataMapper.PlayerItemEntityMapper(reader));
            }
            return main;
        }
        catch (SqliteException ex)
        {
            return null;
        }
    }

    
    public async Task Init()
    {
        const string query = """
            INSERT INTO main (id, level_id) 
            SELECT 1, 1 
            WHERE NOT EXISTS (SELECT 1 FROM main)
         """;
    
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }
}