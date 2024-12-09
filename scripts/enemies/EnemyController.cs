using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyController : CharacterBody3D, ISaveable<TransformSaveData> {
  private SaveableNode<TransformSaveData> saveable;
  public ISaveableUntyped Saveable => saveable;

  public override void _Ready() {
    saveable = SaveableNode<TransformSaveData>.Create(this);
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

  public void OnBeforeLoadGame() {
    GetParent().RemoveChild(this);
    QueueFree();
  }
}
