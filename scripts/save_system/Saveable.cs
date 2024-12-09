using System;
using Godot;

namespace SaveSystem {
  public interface ISaveableBase {
    public abstract ISaveable Saveable { get; }
    public abstract ISaveable InstantiateSaveable();
  }

  public interface ISaveable : ISaveableBase {
    public abstract SaveData OnSaveGame();
    public abstract void OnLoadGame(SaveData data);
    public abstract void OnBeforeLoadGame();
  }

  public interface ISaveable<T> : ISaveableBase where T : SaveData {
    public abstract T OnSaveGame();
    public abstract void OnLoadGame(T data);
  }

  public partial class SaveableNode<T> : Node, ISaveable where T : SaveData {
    public string Identifier { get; private set; } = Guid.NewGuid().ToString();
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

    public void OnLoadGame(SaveData data) {
      Identifier = data.Identifier;
      if (data is T typedData) {
        saveableParent.OnLoadGame(typedData);
      } else {
        throw new ArgumentException($"data could not be typed to '{typeof(T)}', as it is of type '{data.GetType()}'");
      }
    }

    public ISaveable InstantiateSaveable() {
      return this;
    }
  }
}
