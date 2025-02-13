using Godot;

public partial class Player : CharacterBody3D
{
    public const float Speed = 5.0f;

    [Export]
    public float JumpHeight = 1.0f;

    [Export]
    public int MaxHitPoins = 100;

    public int HitPoints;

    private Vector2 _mouseMotion = Vector2.Zero;
    private Node3D _cameraPivot;
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _cameraPivot = GetNode<Node3D>("%CameraPivot");
        HitPoints = MaxHitPoins;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        HandleCameraRotation();

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            GD.Print(_gravity);
            velocity.Y = Mathf.Sqrt(JumpHeight * 2.0f * _gravity);
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {

            if (Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                var mouseEvent = (InputEventMouseMotion)@event;
                _mouseMotion = -mouseEvent.Relative * 0.001f;
            }
        }

        if (@event.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

    }

    public void SetHitPoints(int value)
    {
        HitPoints = value;
        GD.Print(HitPoints);

        if (HitPoints <= 0)
        {
            GetTree().Quit();
        }
    }

    private void HandleCameraRotation()
    {
        RotateY(_mouseMotion.X);
        _cameraPivot.RotateX(_mouseMotion.Y);
        _mouseMotion = Vector2.Zero;
        _cameraPivot.RotationDegrees =
          new Vector3(
            Mathf.Clamp(_cameraPivot.RotationDegrees.X, -90.0f, 90.0f),
           _cameraPivot.RotationDegrees.Y,
           _cameraPivot.RotationDegrees.Z
          );
    }
}
