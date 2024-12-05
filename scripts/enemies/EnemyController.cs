using Godot;
using System;

public partial class EnemyController : CharacterBody3D {
  public override void _Ready() {
    TimeManager.Instance.OnHourUpdated += Despawn;
  }

  private void Despawn(ulong hour) {
    if (hour == 6ul) {
      QueueFree();
    }
  }

  public override void _Process(double delta) {
  }

  public override void _ExitTree() {
    TimeManager.Instance.OnHourUpdated -= Despawn;
  }
}
