using System;
using Godot;
using Newtonsoft.Json.Linq;

namespace SaveSystem {
  public class SaveData {
    public string Identifier;
    public string ScenePath;

    public virtual SaveData ApplyData(JObject data) {
      Identifier = data.GetPropertyOrDefault("Identifier", Guid.NewGuid().ToString());
      ScenePath = data.GetPropertyOrDefault<string>("ScenePath", null);
      return this;
    }
  }
}