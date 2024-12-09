using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Godot;

public interface ISaveableBase {
  public abstract ISaveable Saveable { get; set; }
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
  public string identifier { get; private set; } = Guid.NewGuid().ToString();

  public ISaveable<T> saveableParent;

  public ISaveable Saveable { get => this; set => _ = this; }

  public static SaveableNode<T> Create(Node parent) {
    SaveableNode<T> saveable = new();

    if (parent is ISaveable<T> saveableParent) {
      saveable.saveableParent = saveableParent;
      saveableParent.Saveable = saveable;
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
    data.Identifier = identifier;
    data.ScenePath = ((Node)saveableParent).SceneFilePath;

    return data;
  }

  public void OnBeforeLoadGame() {
    Node saveableParentNode = (Node)saveableParent;
    saveableParentNode.GetParent().RemoveChild(saveableParentNode);
    saveableParentNode.QueueFree();
  }

  public void OnLoadGame(SaveData data) {
    identifier = data.Identifier;
    if (data is T typedData) {
      saveableParent.OnLoadGame(typedData);
    } else {
      throw new ArgumentException($"data could not be typed to '{typeof(T)}', was '{data.GetType()}' instead");
    }
  }

  public ISaveable InstantiateSaveable() {
    return this;
  }
}