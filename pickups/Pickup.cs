using Godot;

public partial class Pickup : Area3D
{
    [Export]
    public AmmoType AmmoType;

    [Export]
    public int Amount = 20;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("player"))
        {
            var player = body as Player;
            player.AmmoHandler.AddAmmo(AmmoType, Amount);
            QueueFree();
        }
    }
}
