using Godot;
using System;

public partial class DayNightManager : Node {
  [ExportGroup("References")]
  [Export] private DirectionalLight3D sun;
  [Export] private DirectionalLight3D moon;

  [ExportGroup("Settings")]
  [Export] private float sunMaxEnergy = 1f;
  [Export] private float moonMaxEnergy = 0.5f;

  public static DayNightManager Instance { get; private set; }

  private float dayProgress = 0f;

  private const float totalMillisecondsInDay = 24 * 60 * 60 * 1000;

  public override void _Ready() {
    if (Instance != null) {
      QueueFree();
      return;
    }

    Instance = this;

    TimeManager.Instance.OnHourUpdated += (hour) => GD.Print($"hour: {hour}");

    sun.RotationDegrees = new Vector3(0f, 90f, 0f);
    moon.RotationDegrees = new Vector3(0f, 90f, 0f);

    CalculateDayProgress();
  }

  public override void _Process(double doubleDelta) {
    float delta = (float)doubleDelta;

    CalculateDayProgress();

    sun.RotationDegrees = new Vector3(90f + dayProgress * 360f, sun.RotationDegrees.Y, sun.RotationDegrees.Z);
    moon.RotationDegrees = new Vector3(90f + (dayProgress + 0.5f) * 360f, moon.RotationDegrees.Y, moon.RotationDegrees.Z);

    sun.LightEnergy = Mathf.Clamp(sun.Transform.Basis.Z.Dot(Vector3.Up), 0f, 1f) * sunMaxEnergy;
    moon.LightEnergy = Mathf.Clamp(moon.Transform.Basis.Z.Dot(Vector3.Up), 0f, 1f) * moonMaxEnergy;
  }

  private void CalculateDayProgress() {
    float currentMillisecondsInDay = ((TimeManager.Instance.Hour * 60 + TimeManager.Instance.Minute) * 60 + TimeManager.Instance.Second) * 1000 + (float)TimeManager.Instance.Millisecond;

    dayProgress = currentMillisecondsInDay / totalMillisecondsInDay;
  }
}
