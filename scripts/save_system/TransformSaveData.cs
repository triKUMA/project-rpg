using Godot;
using Newtonsoft.Json.Linq;
using SaveSystem;

public class TransformSaveData : SaveData {
  public Vector3 Position;
  public Vector3 Rotation;

  public override TransformSaveData ApplyData(JObject data) {
    base.ApplyData(data);

    Position = data.GetPropertyOrDefault("Position", Vector3.Zero);
    Rotation = data.GetPropertyOrDefault("Rotation", Vector3.Zero);

    return this;
  }
}