using System.Collections.Generic;
using Godot;
using Newtonsoft.Json.Linq;

public class PlayerSaveData : TransformSaveData {
  public Vector3 CameraRotation;

  public override void ApplyData(JObject data) {
    base.ApplyData(data);
    CameraRotation = data.GetProperty("CameraRotation", new Vector3(-25, -180, 0));
  }
}