using Godot;

public partial class SmoothCamera : Camera3D
{
    [Export]
    public float Speed = 44.0f;

    private Node3D _cameraPivot;

    public override void _Ready()
    {
        _cameraPivot = GetNode<Node3D>("%CameraPivot");
    }

    public override void _PhysicsProcess(double delta)
    {
        float weight = (float)Mathf.Clamp(delta * Speed, 0.0f, 1.0f);

        GlobalTransform =
            GlobalTransform.InterpolateWith(_cameraPivot.GlobalTransform, weight);

        GlobalPosition = _cameraPivot.GlobalPosition;
    }
}
