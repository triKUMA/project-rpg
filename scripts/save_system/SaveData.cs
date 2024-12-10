using System;
using Newtonsoft.Json.Linq;

namespace SaveSystem {
  public class SaveData : ExternalSaveData {
    public string Identifier;
    public string ScenePath;

    public virtual SaveData ApplyData(JObject data) {
      Identifier = data.GetPropertyOrDefault("Identifier", Guid.NewGuid().ToString());
      ScenePath = data.GetPropertyOrDefault<string>("ScenePath", null);
      _External = data.GetPropertyOrDefault<string>("_External", null);
      return this;
    }
  }

  public class ExternalSaveData {
    // Set _External to save this specific SaveData to a separate file. The string value should be the file path to save
    // to. This can be useful for saving specifically the player's data to a separate file.
    public string _External = null;
  }
}