using Godot;
using SaveSystem;

public partial class TransformSaveData : SaveData {
  [Export] public Vector3 Position;
  [Export] public Vector3 Rotation;
}