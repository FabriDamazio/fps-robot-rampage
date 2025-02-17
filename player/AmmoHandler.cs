using Godot;
using System.Collections.Generic;

public partial class AmmoHandler : Node
{
    public Dictionary<AmmoType, int> AmmoStorage = new Dictionary<AmmoType, int>();

    public override void _Ready()
    {
        AmmoStorage.Add(AmmoType.BULLET, 10);
        AmmoStorage.Add(AmmoType.SMALL_BULLET, 60);
    }

    public bool HasAmmo(AmmoType type)
    {
        return AmmoStorage[type] > 0;
    }

    public void UseAmmo(AmmoType type)
    {
        if (HasAmmo(type))
        {
            AmmoStorage[type] -= 1;
            GD.Print(AmmoStorage[type]);
        }
    }

}

public enum AmmoType
{
    BULLET,
    SMALL_BULLET
}
