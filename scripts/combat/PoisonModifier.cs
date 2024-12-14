using System;
using Godot;
using SaveSystem;

namespace HealthSystem {
  public partial class PoisonModifier : Node {
    [Export] private float damage;

    private HealthController healthController;

    private float timer = 0f;

    public override void _Ready() {
      healthController = (HealthController)GetParent();
      healthController.HealthModifiers.Add(Guid.NewGuid().ToString(), ApplyPoison);
    }

    public float ApplyPoison(float health, float delta) {
      timer += delta;

      if (timer >= 1f) {
        timer -= 1f;

        return health - damage;
      } else {
        return health;
      }
    }
  }
}