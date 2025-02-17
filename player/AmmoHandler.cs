using Godot;
using System.Collections.Generic;

public partial class AmmoHandler : Node
{
    [Export]
    public Label AmmoLabel;

    public Dictionary<AmmoType, int> AmmoStorage =
      new Dictionary<AmmoType, int> {{AmmoType.BULLET, 10}, {AmmoType.SMALL_BULLET, 60}};

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
      GD.Print(type);
        AmmoLabel.Text = AmmoStorage[type].ToString();
    }

}

public enum AmmoType
{
    BULLET,
    SMALL_BULLET
}
