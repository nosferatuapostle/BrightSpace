using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Fighter : SpaceShip
{
    public Fighter(Faction faction) : base(faction)
    {
        name = "Fighter";
        type = UnitType.Fighter;

        radius = 20f;

        var size = 64;
        this.size = new Vector2(size);

        if (faction == Faction.IronCorps)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_ironcorps_fighter", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_ironcorps_fighter_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            deathSprite = Utils.CreateAnimatedSprite("unit_ironcorps_fighter_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7], "base", false);
        }
        else if (faction == Faction.Biomantes)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_biomantes_fighter", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8]);
            engineSprite = Utils.CreateAnimatedSprite("unit_biomantes_fighter_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_biomantes_fighter_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7], "base", false);
        }
        else
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_duskfleet_fighter", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_duskfleet_fighter_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_duskfleet_fighter_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16], "base", false);
        }

        AddEffect(new FighterEffect(this));
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