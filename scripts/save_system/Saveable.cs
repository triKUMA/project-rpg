using System;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SaveSystem {
  public interface ISaveableBase {
    public abstract ISaveable Saveable { get; }
    public abstract ISaveable InstantiateSaveable();
  }

  public interface ISaveable : ISaveableBase {
    public abstract SaveData OnSaveGame();
    public abstract void OnLoadGame(string data);
    public abstract void OnBeforeLoadGame();
  }

  public interface ISaveable<T> : ISaveableBase where T : SaveData {
    public abstract T OnSaveGame();
    public abstract void OnLoadGame(T data);
  }

  public partial class SaveableNode<T> : Node, ISaveable where T : SaveData, new() {
    public string Identifier { get; private set; }
    public ISaveable Saveable => this;

    private ISaveable<T> saveableParent;

    public static SaveableNode<T> Create(Node parent) {
      SaveableNode<T> saveable = new();

      if (parent is ISaveable<T> saveableParent) {
        saveable.saveableParent = saveableParent;
        parent.AddChild(saveable);
      } else {
        throw new ArgumentException($"parent does not implement '{typeof(ISaveable<T>)}'");
      }

      return saveable;
    }

    public override void _Ready() {
      SaveSystemManager.Instance.Saveables.Add(this);
    }

    public override void _ExitTree() {
      SaveSystemManager.Instance.Saveables.Remove(this);
    }

    public SaveData OnSaveGame() {
      SaveData data = saveableParent.OnSaveGame();
      data.Identifier = Identifier;
      data.ScenePath = ((Node)saveableParent).SceneFilePath;

      return data;
    }

    public void OnBeforeLoadGame() {
      Node saveableParentNode = (Node)saveableParent;
      saveableParentNode.GetParent().RemoveChild(saveableParentNode);
      saveableParentNode.QueueFree();
    }

    public void OnLoadGame(string _data) {
      JObject data = JsonConvert.DeserializeObject<JObject>(_data);

      T typedData = new();
      typedData.ApplyData(data);

      Identifier = typedData.Identifier;

      saveableParent.OnLoadGame(typedData);
    }

    public ISaveable InstantiateSaveable() {
      return this;
    }
  }
}
