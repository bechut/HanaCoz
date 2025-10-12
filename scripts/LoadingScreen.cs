using Godot;
using System;
using System.Threading.Tasks;
using HanaCoz.Helpers;

namespace HanaCoz.Scripts;

public partial class LoadingScreen : Control
{
    [Export] public string NextScenePath = "res://scenes/main.tscn";

    private Label _status;
    private ProgressBar _progress;
    private Label _title;

    public override void _Ready()
    {
        // Background color
        var bg = GetNodeOrNull<ColorRect>("ColorRect");
        if (bg != null)
            bg.Color = new Color(0.1f, 0.1f, 0.15f, 1f); // dark blue-gray

        _title = GetNodeOrNull<Label>("VBoxContainer/Label");
        _progress = GetNodeOrNull<ProgressBar>("VBoxContainer/ProgressBar");
        _status = GetNodeOrNull<Label>("VBoxContainer/Status");

        if (_title == null || _progress == null || _status == null)
        {
            GD.PrintErr("⚠️ Missing child nodes in LoadingScreen!");
            return;
        }

        // Title styling
        _title.Text = "LOADING...";
        _title.AddThemeColorOverride("font_color", new Color(0.8f, 0.9f, 1f));
        _title.AddThemeFontSizeOverride("font_size", 32);

        // Status text styling
        _status.Text = "Preparing...";
        _status.AddThemeColorOverride("font_color", new Color(0.7f, 0.8f, 1f));
        _status.AddThemeFontSizeOverride("font_size", 18);

        // ProgressBar styling
        _progress.MinValue = 0;
        _progress.MaxValue = 100;
        _progress.Value = 0;
        _progress.AddThemeColorOverride("fg_color", new Color(0.3f, 0.6f, 1f));
        _progress.AddThemeColorOverride("bg_color", new Color(0.2f, 0.2f, 0.3f));
        
        _ = LoadGameAsync();
    }

    private async Task LoadGameAsync()
    {
        await RunSqlStep("Init items...", () => Global.ItemService.Init(), 25);
        await RunSqlStep("Init player...", () => Global.PlayerService.Init(), 25);
        await RunSqlStep("Init storage...", () => Global.StorageService.Init(), 25);
        await RunSqlStep("Init level...", () => Global.LevelService.Init(), 25);
        await RunSqlStep("Init main...", () => Global.MainService.Init(), 25);

        _status.Text = "Loading main scene...";
        var packedScene = await Task.Run(() => ResourceLoader.Load<PackedScene>(NextScenePath));

        if (packedScene == null)
        {
            GD.PrintErr($"❌ Failed to load scene: {NextScenePath}");
            _status.Text = "Error: Failed to load main scene.";
            return;
        }

        _progress.Value = 100;
        _status.Text = "Starting game...";
        await Task.Delay(300);

        GetTree().ChangeSceneToPacked(packedScene);
    }
    
    private async Task RunSqlStep(string message, Func<Task> action, double target)
    {
        try
        {
            _status.Text = message;
            var start = _progress.Value;
            var progress = start;

            var sqlTask = Task.Run(action);

            while (!sqlTask.IsCompleted)
            {
                progress = Mathf.Lerp((float)progress, (float)target, 0.05f);
                _progress.Value = progress;
                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            }

            await sqlTask;
            _progress.Value = target;
        }
        catch (Exception e)
        {
            GD.Print(e);
        }
    }
}
