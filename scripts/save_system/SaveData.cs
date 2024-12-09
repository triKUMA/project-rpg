using System;
using Godot;
using Newtonsoft.Json.Linq;

namespace SaveSystem {
  public class SaveData {
    public string Identifier;
    public string ScenePath;

    public virtual void ApplyData(JObject data) {
      Identifier = data.GetProperty("Identifier", Guid.NewGuid().ToString());
      ScenePath = data.GetProperty<string>("ScenePath", null);
    }
  }
}