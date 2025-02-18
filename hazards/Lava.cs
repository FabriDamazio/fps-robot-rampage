using Godot;

public partial class Lava : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("player"))
        {
            var player = body as Player;
            player.SetHitPoints(0);
        }

        if (body is Enemy)
        {
            var enemy = body as Enemy;
            enemy.SetHitPoints(0);
        }
    }
}
