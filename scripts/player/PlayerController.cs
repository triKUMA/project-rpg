using Godot;
using Newtonsoft.Json.Linq;
using SaveSystem;

public class PlayerSaveData : TransformSaveData {
  public Vector3 CameraRotation;

  public override PlayerSaveData ApplyData(JObject data) {
    base.ApplyData(data);
    CameraRotation = data.GetPropertyOrDefault("CameraRotation", new Vector3(-25, -180, 0));
    return this;
  }
}

public partial class PlayerController : CharacterBody3D, ISaveable<PlayerSaveData> {
  [Export] private float movementSpeed = 6f;
  [Export] private float acceleration = 40f;
  [Export] private float jumpImpulse = 12f;
  [Export] private float gravity = -30f;

  public Vector3 LastMovementDirection { get; private set; } = Vector3.Back;
  public bool IsMoving => desiredVelocity.LengthSquared() > (movingThreshold * movingThreshold);

  private Vector3 desiredVelocity = Vector3.Zero;
  private Vector3 velocity = Vector3.Zero;
  private float movingThreshold = 0.2f;
  private bool isStartingJump = false;

  private CameraController camera;
  private PlayerSkinController skin;

  private SaveableNode<PlayerSaveData> saveable;
  public ISaveable Saveable => saveable;


  public override void _Ready() {
    InstantiateSaveable();
    camera = this.GetChildByType<CameraController>();
    skin = this.GetChildByType<PlayerSkinController>();
  }

  public override void _Process(double delta) {
    var rawInput = Input.GetVector("move_left", "move_right", "move_backwards", "move_forwards");

    var forward = camera.GlobalBasis.Z;
    var right = camera.GlobalBasis.X;

    desiredVelocity = right * rawInput.X + forward * -rawInput.Y;
    desiredVelocity.Y = 0;
    desiredVelocity = desiredVelocity.Normalized();

    isStartingJump |= Input.IsActionJustPressed("jump") && IsOnFloor();
  }

  public override void _PhysicsProcess(double doubleDelta) {
    float delta = (float)doubleDelta;

    var yVelocity = Velocity.Y;
    Velocity = new Vector3(Velocity.X, 0f, Velocity.Z);
    Velocity = Velocity.MoveToward(desiredVelocity * movementSpeed, acceleration * delta);
    Velocity = new Vector3(Velocity.X, yVelocity + gravity * delta, Velocity.Z);

    if (isStartingJump) {
      isStartingJump = false;
      Velocity = new Vector3(Velocity.X, Velocity.Y + jumpImpulse, Velocity.Z);
    }

    MoveAndSlide();

    if (desiredVelocity.LengthSquared() > movingThreshold * movingThreshold) {
      LastMovementDirection = desiredVelocity;
    }
  }

  public PlayerSaveData OnSaveGame() {
    return new() {
      Position = Position,
      Rotation = skin.RotationDegrees,
      CameraRotation = camera.Pivot.RotationDegrees,
    };
  }

  public void OnLoadGame(PlayerSaveData data) {
    Position = data.Position;
    skin.RotationDegrees = data.Rotation;
    camera.Pivot.RotationDegrees = data.CameraRotation;
  }

  public ISaveable InstantiateSaveable() {
    if (saveable == null) {
      saveable = SaveableNode<PlayerSaveData>.Create(this);
    }

    return saveable;
  }
}
