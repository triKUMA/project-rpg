using Godot;

namespace SaveSystem {
  public partial class SaveData : Resource {
    [Export] public string Identifier;
    [Export] public string ScenePath;
  }
}