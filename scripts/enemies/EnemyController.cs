using Godot;
using SaveSystem;

public partial class EnemyController : CharacterBody3D, ISaveable<TransformSaveData> {
  private SaveableNode<TransformSaveData> saveable;
  public ISaveable Saveable => saveable;

  public override void _EnterTree() {
    InstantiateSaveable();
  }

  public override void _Process(double dDelta) {
  }

  public TransformSaveData OnSaveGame() {
    return new() {
      Position = Position,
      Rotation = RotationDegrees
    };
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
