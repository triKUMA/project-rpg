using Godot;
using System;

public partial class PlayerSkinController : Node3D {
  [Export] private float rotationSpeed = 12f;

  private PlayerController player;

  public override void _Ready() {
    player = (PlayerController)GetParent();
  }

  public override void _Process(double doubleDelta) {
    if (!player.IsMoving) return;

    float delta = (float)doubleDelta;

    var targetAngle = Vector3.Back.SignedAngleTo(player.LastMovementDirection, Vector3.Up);
    GlobalRotation = Vector3.Up * Mathf.LerpAngle(GlobalRotation.Y, targetAngle, rotationSpeed * delta);
  }
}
