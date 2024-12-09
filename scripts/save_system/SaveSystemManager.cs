using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class SaveSystemManager : Node {
  public static SaveSystemManager Instance { get; private set; }
  public List<ISaveableUntyped> Saveables { get; private set; } = new();

  private string saveFilePath = "user://savegame.tres";

  public override void _Ready() {
    if (Instance != null) {
      QueueFree();
      return;
    }

    Instance = this;
  }

  public override void _Process(double dDelta) {
  }

  private void SaveGame() {
    GD.Print($"Saving game to '{ProjectSettings.GlobalizePath(saveFilePath)}'...");

    GameSaveData gameData = new GameSaveData();

    List<SaveData> saveablesData = new();
    foreach (ISaveableUntyped saveable in Saveables) {
      saveable.OnSaveGame(saveablesData, null);
    }
    gameData.Data = saveablesData.ToArray();

    ResourceSaver.Save(gameData, saveFilePath, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);
  }

  private void LoadGame() {
    ClearGame();

    GD.Print($"Loading game from '{ProjectSettings.GlobalizePath(saveFilePath)}'...");
    GameSaveData gameData = ResourceLoader.Load<GameSaveData>(saveFilePath, null, ResourceLoader.CacheMode.IgnoreDeep);
    foreach (SaveData data in gameData.Data) {
      // load scene from data.scenePath
      PackedScene scene = ResourceLoader.Load<PackedScene>(data.ScenePath);
      // cast loaded scene to ISaveable and call OnLoadGame on ISaveable, passing in data
      Node instance = scene.Instantiate();
      if (instance is ISaveableBase saveableBase) {
        instance.CallDeferred("OnLoadGame", data);
        // saveableBase.Saveable.OnLoadGame(data); // Pass the data to the saveable instance
      }

      GetTree().Root.GetChild(0).AddChild(instance);
    }
  }

  private void ClearGame() {
    GD.Print("Clearing game of all existing loadables...");
    ISaveableUntyped[] oldSaveables = Saveables.ToArray();
    foreach (ISaveableUntyped saveable in oldSaveables) {
      saveable.OnBeforeLoadGame();
    }
  }

  public override void _Input(InputEvent @event) {
    if (@event is InputEventKey keyEvent && keyEvent.Pressed && Input.IsKeyPressed(Key.Ctrl)) {
      if (Input.IsKeyPressed(Key.S)) {
        SaveGame();
        GD.Print("Game has been saved!");
      } else if (Input.IsKeyPressed(Key.L)) {
        LoadGame();
        GD.Print("Game has been loaded from save!");
      }
    }
  }
}
