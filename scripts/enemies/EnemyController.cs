using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyController : CharacterBody3D, ISaveable<TransformSaveData> {
  private SaveableNode<TransformSaveData> saveable;
  public ISaveable Saveable { get => saveable; set => saveable = (SaveableNode<TransformSaveData>)value; }

  public override void _Ready() {
    InstantiateSaveable();
  }

  public override void _Process(double dDelta) {
  }

  public void OnSaveGame(List<SaveData> data, string identifier) {
    TransformSaveData saveData = new() {
      Identifier = identifier,
      ScenePath = SceneFilePath,
      Position = Position,
      Rotation = RotationDegrees
    };

    data.Add(saveData);
  }

  public void OnLoadGame(TransformSaveData data) {
    Position = data.Position;
    RotationDegrees = data.Rotation;
  }

  public ISaveable InstantiateSaveable() {
    if (saveable == null) {
      saveable = SaveableNode<TransformSaveData>.Create(this);
    }

    return saveable;
  }
}
