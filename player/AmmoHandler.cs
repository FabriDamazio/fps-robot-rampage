using Godot;
using System.Collections.Generic;

public partial class AmmoHandler : Node
{
    [Export]
    public Label AmmoLabel;

    public Dictionary<AmmoType, int> AmmoStorage =
      new Dictionary<AmmoType, int> { { AmmoType.BULLET, 10 }, { AmmoType.SMALL_BULLET, 60 } };

    public override void _Ready()
    {
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
            UpdateAmmoLabel(type);
        }
    }

    public void UpdateAmmoLabel(AmmoType type)
    {
        AmmoLabel.Text = AmmoStorage[type].ToString();
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        AmmoStorage[type] += amount;
        UpdateAmmoLabel(type);
    }


}

public enum AmmoType
{
    BULLET,
    SMALL_BULLET
}
