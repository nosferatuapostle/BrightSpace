using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Battlecruiser : SpaceShip
{
    public Battlecruiser(Faction faction) : base(faction)
    {
        name = "Battlecruiser";
        type = UnitType.Battlecruiser;

        radius = 36f;
        extraHeight = 12f;

        var size = 128;
        this.size = new Vector2(size);

        if (faction == Faction.IronCorps)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_ironcorps_battlecruiser", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29]);
            engineSprite = Utils.CreateAnimatedSprite("unit_ironcorps_battlecruiser_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]);
            deathSprite = Utils.CreateAnimatedSprite("unit_ironcorps_battlecruiser_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], "base", false);
        }
        else if (faction == Faction.Biomantes)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_biomantes_battlecruiser", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8]);
            engineSprite = Utils.CreateAnimatedSprite("unit_biomantes_battlecruiser_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_biomantes_battlecruiser_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11], "base", false);
        }
        else
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_duskfleet_battlecruiser", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8]);
            engineSprite = Utils.CreateAnimatedSprite("unit_duskfleet_battlecruiser_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_duskfleet_battlecruiser_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17], "base", false);
        }

        AddEffect(new BattlecruiserEffect(this));
    }

    public override void LevelUp(float mult = 1)
    {
        GetValueModifier(UnitValue.Health, ModifierData.PER_LEVEL).value += 1f * mult;
        GetValueModifier(UnitValue.Magnitude, ModifierData.PER_LEVEL).value += 0.125f * mult;
        GetValueModifier(UnitValue.SpeedMult, ModifierData.PER_LEVEL).value += 0.04f * mult;
        GetValueModifier(UnitValue.Range, ModifierData.PER_LEVEL).value += 1f * mult;
        base.LevelUp(mult);
    }
}