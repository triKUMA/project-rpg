using Godot;

namespace SaveSystem {
  public partial class GameSaveData : Resource {
    [Export] public SaveData[] Data;
  }
}