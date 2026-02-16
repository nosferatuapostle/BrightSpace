using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Dreadnought : SpaceShip
{
    public Dreadnought(Faction faction) : base(faction)
    {
        name = "Dreadnought";
        type = UnitType.Dreadnought;

        radius = 48f;
        extraHeight = 12f;

        var size = 128;
        this.size = new Vector2(size);

        if (faction == Faction.IronCorps)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_ironcorps_dreadnought", size, size, 60);
            engineSprite = Utils.CreateAnimatedSprite("unit_ironcorps_dreadnought_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]);
            deathSprite = Utils.CreateAnimatedSprite("unit_ironcorps_dreadnought_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11], "base", false);
        }
        else if (faction == Faction.Biomantes)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_biomantes_dreadnought", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34]);
            engineSprite = Utils.CreateAnimatedSprite("unit_biomantes_dreadnought_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_biomantes_dreadnought_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11], "base", false);
        }
        else
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_duskfleet_dreadnought", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33]);
            engineSprite = Utils.CreateAnimatedSprite("unit_duskfleet_dreadnought_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_duskfleet_dreadnought_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17], "base", false);
        }

        AddEffect(new DreadnoughtEffect(this));
    }

    public override void LevelUp(float mult = 1f)
    {
        GetValueModifier(UnitValue.Health, ModifierData.PER_LEVEL).value += 1f * mult;
        GetValueModifier(UnitValue.Magnitude, ModifierData.PER_LEVEL).value += 0.125f * mult;
        GetValueModifier(UnitValue.SpeedMult, ModifierData.PER_LEVEL).value += 0.04f * mult;
        GetValueModifier(UnitValue.Range, ModifierData.PER_LEVEL).value += 1f * mult;
        base.LevelUp();
    }
}