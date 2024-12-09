using System;
using System.Collections.Generic;
using Godot;

public interface ISaveableBase {
  public abstract ISaveableUntyped Saveable { get; }
  public abstract void OnSaveGame(List<SaveData> data, string identifier);
  public abstract void OnBeforeLoadGame();
}

public interface ISaveableUntyped : ISaveableBase {
  public abstract void OnLoadGame(SaveData data);
}

public interface ISaveable<T> : ISaveableBase where T : SaveData {
  public abstract void OnLoadGame(T data);
}

public partial class SaveableNode<T> : Node, ISaveableUntyped where T : SaveData {
  public string identifier { get; private set; } = Guid.NewGuid().ToString();
  private ISaveable<T> saveableParent;
  private string name;

  public ISaveableUntyped Saveable => this;

  public override void _Ready() {
    SaveSystemManager.Instance.Saveables.Add(this);
  }

  public override void _ExitTree() {
    SaveSystemManager.Instance.Saveables.Remove(this);
  }

  public static SaveableNode<T> Create(Node parent) {
    SaveableNode<T> saveable = new();
    saveable.name = parent.Name;

    parent.AddChild(saveable);
    if (parent is ISaveable<T> saveableParent) {
      saveable.saveableParent = saveableParent;
    } else {
      throw new ArgumentException("parent does not implement ISaveable");
    }

    return saveable;
  }

  public void OnSaveGame(List<SaveData> data, string _) => saveableParent.OnSaveGame(data, identifier);
  public void OnBeforeLoadGame() => saveableParent.OnBeforeLoadGame();
  public void OnLoadGame(SaveData data) {
    identifier = data.Identifier;
    if (data is T typedData) {
      saveableParent.OnLoadGame(typedData);
    } else {
      throw new ArgumentException($"data could not be typed to '{typeof(T)}', was '{data.GetType()}' instead");
    }
  }
}