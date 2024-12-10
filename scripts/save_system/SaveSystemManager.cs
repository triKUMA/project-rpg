using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Godot;
using Newtonsoft.Json.Linq;

namespace SaveSystem {
  public partial class SaveSystemManager : Node {
    [Export] private string saveFilePath = "user://game.json";

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
      List<dynamic> externalData = new();
      foreach (ISaveable saveable in Saveables) {
        SaveData saveData = saveable.OnSaveGame();
        if (saveData._External != null) {
          ExternalSaveData externalRef = new() {
            _External = saveData._External
          };
          data.Add(externalRef);

          externalData.Add(saveData);
        } else {
          data.Add(saveData);
        }
      }

      string dataJson = JsonConvert.SerializeObject(data, Formatting.Indented);

      using (FileAccess saveFile = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Write)) {
        saveFile.StoreString(dataJson);
      }

      foreach (dynamic extData in externalData) {
        string externalFilePath = extData._External;
        extData._External = null;
        string extDataJson = JsonConvert.SerializeObject(extData, Formatting.Indented);

        using (FileAccess extSaveFile = FileAccess.Open(externalFilePath, FileAccess.ModeFlags.Write)) {
          extSaveFile.StoreString(extDataJson);
        }
      }
    }

    public void LoadGame() {
      ClearGame();

      JObject[] data = new JObject[] { };
      using (FileAccess saveFile = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Read)) {
        data = JsonConvert.DeserializeObject<JObject[]>(saveFile.GetAsText());
      }

      for (int i = 0; i < data.Length; i++) {
        JObject saveData = data[i];
        string externalFilePath = saveData.GetPropertyOrDefault<string>("_External", null);
        if (externalFilePath != null) {
          using (FileAccess saveFile = FileAccess.Open(externalFilePath, FileAccess.ModeFlags.Read)) {
            saveData = JsonConvert.DeserializeObject<JObject>(saveFile.GetAsText());
          }
          saveData.Remove("_External");
          saveData.Add("_External", externalFilePath);
        }
        PackedScene scene = ResourceLoader.Load<PackedScene>(saveData.GetPropertyOrDefault<string>("ScenePath", null));
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