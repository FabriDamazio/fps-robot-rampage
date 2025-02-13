using Godot;

public partial class HitscanWeapon : Node3D
{
    [Export]
    public float FireRate = 14.0f;

    [Export]
    public float Recoil = 0.05f;

    [Export]
    public Node3D WeaponMesh;

    [Export]
    public GpuParticles3D MuzzleFlash;

    [Export]
    public int WeaponDamage = 15;

    private Timer _cooldownTimer;
    private Vector3 _weaponPosition;
    private RayCast3D _rayCast3D;

    public override void _Ready()
    {
        _cooldownTimer = GetNode<Timer>("%CooldownTimer");
        _weaponPosition = WeaponMesh.Position;
        _rayCast3D = GetNode<RayCast3D>("%RayCast3D");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("fire"))
        {
            if (_cooldownTimer.IsStopped())
            {
                Shoot();
            }
        }

        WeaponMesh.Position = WeaponMesh.Position.Lerp(_weaponPosition, (float)delta * 10);
    }

    public void Shoot()
    {
        MuzzleFlash.Restart();

        _cooldownTimer.Start(1.0f / FireRate);
        GD.Print("Weapon Fired");

        WeaponMesh.Position =
          new Vector3(WeaponMesh.Position.X, WeaponMesh.Position.Y, WeaponMesh.Position.Z + Recoil);
        var collider = _rayCast3D.GetCollider();
        GD.Print(collider);

        if (collider is Enemy)
        {
            var enemy = collider as Enemy;
            enemy.SetHitPoints(enemy.HitPoints - WeaponDamage);
        }
    }
}
