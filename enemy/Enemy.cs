using Godot;

public partial class Enemy : CharacterBody3D
{
    public const float Speed = 5.0f;

    [Export]
    public float AttackRange = 1.5f;

    [Export]
    public int MaxHitPoints = 100;

    [Export]
    public int AttackDamage = 20;

    public int HitPoints;

    private NavigationAgent3D _navigationAgent3D;
    private Player _player;
    private bool _provoked = false;
    private float _aggroRange = 12.0f;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("player") as Player;
        _navigationAgent3D = GetNode<NavigationAgent3D>("%NavigationAgent3D");
        _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
        HitPoints = MaxHitPoints;
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

        if (_provoked && distance <= AttackRange)
            _animationPlayer.Play("attack");

        if (direction != Vector3.Zero)
        {
            LookAtTarget(direction);
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

    private void LookAtTarget(Vector3 direction)
    {
        var adjustedDirection = new Vector3(direction.X, 0.0f, direction.Z);
        LookAt(GlobalPosition + adjustedDirection, Vector3.Up, true);
    }

    public void Attack()
    {
        GD.Print("Enemy Attack");
        _player.SetHitPoints(_player.HitPoints - AttackDamage);

    }

    public void SetHitPoints(int value)
    {
        HitPoints = value;

        if (HitPoints <= 0)
        {
            QueueFree();
        }

        _provoked = true;
    }
}
