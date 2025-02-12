using Godot;

public partial class Enemy : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    private NavigationAgent3D _navigationAgent3D;
    private Player _player;
    private bool _provoked = false;
    private float _aggroRange = 12.0f;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("player") as Player;
        _navigationAgent3D = GetNode<NavigationAgent3D>("%NavigationAgent3D");
    }

    public override void _Process(double delta)
    {
        if (_provoked)
            _navigationAgent3D.TargetPosition = _player.GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 nextPosition = _navigationAgent3D.GetNextPathPosition();

        Vector3 velocity = Velocity;
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        var direction = GlobalPosition.DirectionTo(nextPosition);
        var distance = GlobalPosition.DistanceTo(_player.GlobalPosition);

        if (distance <= _aggroRange)
            _provoked = true;

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
}
