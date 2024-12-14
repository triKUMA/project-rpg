using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthSystem {
  public partial class HealthController : Node, IDamageable {
    [Export] private float baseMaxHealth;

    public event Action<float> OnDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    public Dictionary<string, Func<float, float>> DamageModifiers = new();
    public Dictionary<string, Func<float, float>> HealModifiers = new();
    public Dictionary<string, Func<float, float, float>> HealthModifiers = new();

    public float Health { get; private set; }
    public float MaxHealth => baseMaxHealth;

    public bool IsDead { get; private set; } = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
      Health = baseMaxHealth;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double dDelta) {
      if (IsDead) return;

      float delta = (float)dDelta;

      float prevHealth = Health;
      Health = Mathf.Clamp(ApplyDeltaValueModifiers(Health, delta, HealthModifiers.Values.ToArray()), 0, MaxHealth);

      if (prevHealth != Health) {
        if (prevHealth > Health) {
          OnDamage?.Invoke(prevHealth - Health);
        } else {
          OnHeal?.Invoke(Health - prevHealth);
        }
      }

      if (Health <= 0f) {
        Die();
        OnDeath?.Invoke();
      }
    }

    private float ApplyValueModifiers(float value, Func<float, float>[] modifiers) {
      foreach (var modifier in modifiers) {
        value = modifier(value);
      }

      return value;
    }

    private float ApplyDeltaValueModifiers(float value, float delta, Func<float, float, float>[] modifiers) {
      foreach (var modifier in modifiers) {
        value = modifier(value, delta);
      }

      return value;
    }

    public void Damage(float amount) {
      if (IsDead || amount <= 0) return;

      float modifiedDamage = ApplyValueModifiers(amount, DamageModifiers.Values.ToArray());
      Health = Mathf.Max(Health - modifiedDamage, 0f);

      OnDamage?.Invoke(modifiedDamage);
    }

    public void Heal(float amount) {
      if (IsDead || amount <= 0) return;

      float modifiedHeal = ApplyValueModifiers(amount, HealModifiers.Values.ToArray());
      Health = Mathf.Min(Health + modifiedHeal, MaxHealth);

      OnHeal?.Invoke(modifiedHeal);
    }

    private void Die() {
      IsDead = true;
      GD.Print("you ded");
    }
  }
}
