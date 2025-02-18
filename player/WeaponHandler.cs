using Godot;
using System.Linq;

public partial class WeaponHandler : Node3D
{
    [Export]
    public Node3D Weapon1;

    [Export]
    public Node3D Weapon2;

    public override void _Ready()
    {
        Equip(Weapon1);
    }

    public void Equip(Node3D activeWeapon)
    {
        foreach (var child in GetChildren())
        {
            var weapon = child as HitscanWeapon;

            if (weapon == activeWeapon)
            {
                weapon.Visible = true;
                weapon.SetProcess(true);
                weapon.AmmoHandler.UpdateAmmoLabel(weapon.AmmoType);
            }
            else
            {
                weapon.Visible = false;
                weapon.SetProcess(false);
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("weapon_1"))
        {
            Equip(Weapon1);
        }

        if (@event.IsActionPressed("weapon_2"))
        {
            Equip(Weapon2);
        }

        if (@event.IsActionPressed("next_weapon"))
        {
            NextWeapon();
        }

        if (@event.IsActionPressed("last_weapon"))
        {
            LastWeapon();
        }
    }

    public void NextWeapon()
    {
        var index = GetCurrentIndex();
        index = Mathf.Wrap(index + 1, 0, GetChildCount());
        Equip(GetChild(index) as Node3D);
    }

    public void LastWeapon()
    {
        var index = GetCurrentIndex();
        index = Mathf.Wrap(index - 1, 0, GetChildCount());
        Equip(GetChild(index) as Node3D);
    }

    public AmmoType GetWeaponAmmo()
    {
      var currentWeapon = GetChild(GetCurrentIndex()) as HitscanWeapon;
      return currentWeapon.AmmoType;
    }

    private int GetCurrentIndex()
    {
        foreach (var index in Enumerable.Range(0, GetChildCount()))
        {
            if (GetChild<Node3D>(index).Visible)
                return index;
        }

        return 0;
    }
}
