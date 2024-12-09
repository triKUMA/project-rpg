using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class SaveSystemManager : Node {
  [Export] private string saveFilePath = "user://savegame.tres";

  public static SaveSystemManager Instance { get; private set; }
  public List<ISaveable> Saveables { get; private set; } = new();

  public override void _Ready() {
    if (Instance != null) {
      QueueFree();
      return;
    }

    Instance = this;
  }

  public override void _Input(InputEvent @event) {
    if (@event is InputEventKey keyEvent && keyEvent.Pressed && Input.IsKeyPressed(Key.Ctrl)) {
      if (Input.IsKeyPressed(Key.S)) {
        SaveGame();
      } else if (Input.IsKeyPressed(Key.L)) {
        LoadGame();
      }
    }
  }

  private void SaveGame() {
    GameSaveData gameData = new GameSaveData();

    List<SaveData> saveablesData = new();
    foreach (ISaveable saveable in Saveables) {
      saveable.OnSaveGame(saveablesData, null);
    }
    gameData.Data = saveablesData.ToArray();

    ResourceSaver.Save(gameData, saveFilePath, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);
  }

  private void LoadGame() {
    ClearGame();

    GameSaveData gameData = ResourceLoader.Load<GameSaveData>(saveFilePath, null, ResourceLoader.CacheMode.IgnoreDeep);
    foreach (SaveData data in gameData.Data) {
      PackedScene scene = ResourceLoader.Load<PackedScene>(data.ScenePath);
      Node instance = scene.Instantiate();

      if (instance is ISaveableBase saveableBase) {
        var saveable = saveableBase.InstantiateSaveable();
        ((Node)saveable).CallDeferred("OnLoadGame", data);
      }

      GetTree().Root.GetChild(0).AddChild(instance);
    }
  }

  private void ClearGame() {
    ISaveable[] oldSaveables = Saveables.ToArray();
    foreach (ISaveable saveable in oldSaveables) {
      saveable.OnBeforeLoadGame();
    }
  }
}
