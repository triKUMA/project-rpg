using Godot;
using System;

public partial class EnemySpawner : Node3D {
  [Export] private PackedScene enemyResource;

  public override void _Ready() {
    TimeManager.Instance.OnHourUpdated += (hour) => {
      if (hour == 19ul) SpawnEnemy();
    };
  }

  public override void _Process(double delta) {
  }

  private void SpawnEnemy() {
    Node3D enemy = enemyResource.Instantiate<Node3D>();
    enemy.Position = Position;

    Node3D rootNode = GDExtensions.GetRootNode<Node3D>("Root");
    rootNode.AddChild(enemy);
  }
}
