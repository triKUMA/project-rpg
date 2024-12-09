using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Godot;

namespace SaveSystem {
  public partial class SaveSystemManager : Node {
    [Export] private string saveFilePath = "user://savegame.json";

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

    public void SaveGame() {
      List<dynamic> data = new();
      foreach (ISaveable saveable in Saveables) {
        data.Add(saveable.OnSaveGame());
      }

      string dataJson = JsonConvert.SerializeObject(data, Formatting.Indented);

      using FileAccess saveFile = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Write);
      saveFile.StoreString(dataJson);
    }

    public void LoadGame() {
      ClearGame();

      // GameSaveData gameData = ResourceLoader.Load<GameSaveData>(saveFilePath, null, ResourceLoader.CacheMode.IgnoreDeep);
      using FileAccess saveFile = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Read);
      dynamic[] data = JsonConvert.DeserializeObject<dynamic[]>(saveFile.GetAsText());
      foreach (dynamic saveData in data) {
        PackedScene scene = ResourceLoader.Load<PackedScene>((string)saveData.ScenePath);
        Node instance = scene.Instantiate();

        if (instance is ISaveableBase saveableBase) {
          var saveable = saveableBase.InstantiateSaveable();
          ((Node)saveable).CallDeferred("OnLoadGame", JsonConvert.SerializeObject(saveData));

          GetTree().Root.GetChild(0).AddChild(instance);
        } else {
          throw new TypeAccessException($"expected instance to be of type '{typeof(ISaveableBase)}', but is instead '{instance.GetType()}'");
        }
      }
    }

    private void ClearGame() {
      ISaveable[] oldSaveables = Saveables.ToArray();
      foreach (ISaveable saveable in oldSaveables) {
        saveable.OnBeforeLoadGame();
      }
    }
  }
}