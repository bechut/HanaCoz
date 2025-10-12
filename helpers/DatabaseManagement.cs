using Godot;
using Microsoft.Data.Sqlite;
using System;
using HanaCoz.Helpers.Services;

namespace HanaCoz.Helpers;

public partial class DatabaseManagement : Node
{
    private SqliteConnection _connection;
    private const string DbPath = "user://gamedata.db";
    public static DatabaseManagement Instance { get; private set; }
    
    public override void _Ready()
    {
        Instance = this;
        var absolutePath = ProjectSettings.GlobalizePath(DbPath);
        GD.Print($"Database path: {absolutePath}");

        _connection = new SqliteConnection($"Data Source={absolutePath}");

        try
        {
            _connection.Open();
            GD.Print("Database connection opened successfully.");
            InitializeDatabase();
        }
        catch (Exception e)
        {
            GD.PrintErr($"Database connection failed: {e.Message}");
        }
    }

    public override void _ExitTree()
    {
        _connection?.Close();
        Instance = null;
        GD.Print("Database connection closed.");
    }

    private void InitializeDatabase()
    {
        const string createTableQuery = """
            CREATE TABLE IF NOT EXISTS item (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                code TEXT NOT NULL,
                name TEXT,
                type TEXT,
                region_x INTEGER DEFAULT 34,
                region_y INTEGER DEFAULT 44,
                region_w INTEGER DEFAULT 28,
                region_h INTEGER DEFAULT 20,
                description TEXT,
                UNIQUE (code,type),
                CHECK (type IN ('outfit', 'accessory', 'hairstyle'))
            );
            CREATE TABLE IF NOT EXISTS player (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                code TEXT,
                pos_x INTEGER,
                pos_y INTEGER,
                run_speed INTEGER
            );
            CREATE TABLE IF NOT EXISTS player_item (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                item_id INTEGER,
                player_id INTEGER,
                equipped INTEGER DEFAULT 0,
                type TEXT,
        
                FOREIGN KEY (item_id) REFERENCES item(id),
                FOREIGN KEY (player_id) REFERENCES player(id),
                UNIQUE (item_id,player_id),
                UNIQUE (player_id, type, equipped),
                CHECK (type IN ('outfit', 'accessory', 'hairstyle'))
            );
            CREATE TABLE IF NOT EXISTS storage (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                code TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS storage_item (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                item_order INTEGER NOT NULL,
                item_id INTEGER NOT NULL,
                storage_id INTEGER NOT NULL,
                FOREIGN KEY (item_id) REFERENCES item(id),
                FOREIGN KEY (storage_id) REFERENCES storage(id)
            );
            CREATE TABLE IF NOT EXISTS level (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                no INTEGER NOT NULL,
                player_id TEXT,
                FOREIGN KEY (player_id) REFERENCES player(id)
            );
            CREATE TABLE IF NOT EXISTS main (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                level_id Int,
                FOREIGN KEY (level_id) REFERENCES level(id)
            );
        """;
        try
        {
            using var command = _connection.CreateCommand();
            command.CommandText = createTableQuery;
            command.ExecuteNonQuery();
            GD.Print("Database initialized.");
        }
        catch (Exception e)
        {
            GD.Print("Database error.", e);
            
        }
    }

    public SqliteConnection GetConnection() => _connection;

    public StorageService GetStorageService() => new(_connection);
    public PlayerService GetPlayerService() => new(_connection);
    public LevelService GetLevelService() => new(_connection);
    public MainService GetMainService() => new(_connection);
    public ItemService GetItemService() => new(_connection);
}
