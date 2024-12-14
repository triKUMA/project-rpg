namespace HealthSystem {
  public interface IDamageable {
    public abstract void Damage(float amount);
    public abstract void Heal(float amount);
  }
}