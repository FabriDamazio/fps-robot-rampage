using Godot;

public partial class Player : CharacterBody3D
{
    [Export]
    public float Speed = 8.0f;

    [Export]
    public float JumpHeight = 1.0f;

    [Export]
    public int MaxHitPoins = 100;

    [Export]
    public float AimMultiplier = 0.7f;

    public AmmoHandler AmmoHandler;

    public int HitPoints;

    private Vector2 _mouseMotion = Vector2.Zero;
    private Node3D _cameraPivot;
    private AnimationPlayer _damageAnimationPlayer;
    private GameOverMenu _gameOverMenu;
    private Camera3D _smoothCamera;
    private Camera3D _weaponCamera;
    private float _smoothCameraFOV;
    private float _weaponCameraFOV;
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _cameraPivot = GetNode<Node3D>("%CameraPivot");
        _damageAnimationPlayer = GetNode<AnimationPlayer>("%DamageAnimationPlayer");
        HitPoints = MaxHitPoins;
        _gameOverMenu = GetNode<GameOverMenu>("%GameOverMenu");
        AmmoHandler = GetNode<AmmoHandler>("%AmmoHandler");
        _smoothCamera = GetNode<Camera3D>("%SmoothCamera");
        _smoothCameraFOV = _smoothCamera.Fov;
        _weaponCamera = GetNode<Camera3D>("%WeaponCamera");
        _weaponCameraFOV = _weaponCamera.Fov;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("aim"))
        {
            _smoothCamera.Fov = (float)Mathf.Lerp(_smoothCamera.Fov, _smoothCameraFOV * AimMultiplier, delta * 20);
            _weaponCamera.Fov = (float)Mathf.Lerp(_weaponCamera.Fov, _weaponCameraFOV * AimMultiplier, delta * 20);
        }
        else
        {
            _smoothCamera.Fov = (float)Mathf.Lerp(_smoothCamera.Fov, _smoothCameraFOV, delta * 30);
            _weaponCamera.Fov = (float)Mathf.Lerp(_weaponCamera.Fov, _weaponCameraFOV, delta * 30);
        }
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
            if (Input.IsActionPressed("aim"))
            {
                velocity.X *= AimMultiplier;
                velocity.Z *= AimMultiplier;
            }
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
                if (Input.IsActionPressed("aim"))
                {
                    _mouseMotion *= AimMultiplier;
                }
            }
        }

        if (@event.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

    }

    public void SetHitPoints(int value)
    {
        if (value < HitPoints)
        {
            _damageAnimationPlayer.Stop(false);
            _damageAnimationPlayer.Play("take_damage");
        }
        HitPoints = value;
        GD.Print(HitPoints);

        if (HitPoints <= 0)
        {
            _gameOverMenu.GameOver();
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
