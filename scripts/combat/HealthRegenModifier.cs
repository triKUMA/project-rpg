using System;
using Godot;
using SaveSystem;

namespace HealthSystem {
  public partial class HealthRegenModifier : Node {
    [Export] private float healthRegenPerSec;

    private HealthController healthController;

    public override void _Ready() {
      healthController = (HealthController)GetParent();
      healthController.HealthModifiers.Add(Guid.NewGuid().ToString(), HealPerSec);
    }

    public float HealPerSec(float health, float delta) {
      return health + (healthRegenPerSec * delta);
    }
  }
}