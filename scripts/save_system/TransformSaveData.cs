using Godot;
using Newtonsoft.Json.Linq;
using SaveSystem;

public class TransformSaveData : SaveData {
  public Vector3 Position;
  public Vector3 Rotation;

  public override void ApplyData(JObject data) {
    base.ApplyData(data);
    Position = data.GetProperty("Position", Vector3.Zero);
    Rotation = data.GetProperty("Rotation", Vector3.Zero);
  }
}