using Godot;

public partial class CameraController : Camera3D {
  [Export] public float RotationSpeed = 5f;
  [Export] public Vector2 RotationClamp = new Vector2(-30f, 60f);

  public Node3D Pivot { get; private set; }

  private Vector2 inputDirection = Vector2.Zero;

  public override void _Ready() {
    Pivot = (Node3D)GetParent();
  }

  public override void _Process(double doubleDelta) {
    float delta = (float)doubleDelta;
    Pivot.RotationDegrees = new Vector3(
      Mathf.Clamp(Pivot.RotationDegrees.X - inputDirection.Y * delta, -90f - RotationClamp.X, 90f - RotationClamp.Y),
      Pivot.RotationDegrees.Y - inputDirection.X * delta,
      Pivot.RotationDegrees.Z
    );

    inputDirection = Vector2.Zero;
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (
      @event is not InputEventMouseMotion eventMouseMotion ||
      Input.MouseMode != Input.MouseModeEnum.Captured
    ) return;

    inputDirection = eventMouseMotion.ScreenRelative * RotationSpeed;
  }

  public override void _Input(InputEvent @event) {
    if (@event.IsActionPressed("left_click")) {
      Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    if (@event.IsActionPressed("ui_cancel")) {
      Input.MouseMode = Input.MouseModeEnum.Visible;
    }
  }
}
