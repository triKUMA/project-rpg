using System.Collections.Generic;
using Godot;
using Newtonsoft.Json.Linq;

public class PlayerSaveData : TransformSaveData {
  public Vector3 CameraRotation;

  public override PlayerSaveData ApplyData(JObject data) {
    base.ApplyData(data);

    CameraRotation = data.GetPropertyOrDefault("CameraRotation", new Vector3(-25, -180, 0));

    return this;
  }
}