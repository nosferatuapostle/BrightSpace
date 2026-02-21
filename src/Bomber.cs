using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Bomber : SpaceShip
{
    public Bomber(Faction faction) : base(faction, 20f)
    {
        name = "Bomber";
        type = UnitType.Bomber;

        radius = 18f;

        var size = Data.UNIT_SIZE_64;
        this.size = new Vector2(size);

        if (faction == Faction.IronCorps)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_ironcorps_bomber", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_ironcorps_bomber_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            deathSprite = Utils.CreateAnimatedSprite("unit_ironcorps_bomber_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7], "base", false);
        }
        else if (faction == Faction.Biomantes)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_biomantes_bomber", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_biomantes_bomber_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_biomantes_bomber_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8], "base", false);
        }
        else
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_duskfleet_bomber", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_duskfleet_bomber_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_duskfleet_bomber_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15], "base", false);
        }

        baseSpeed = 100f;

        SetBaseValue(UnitValue.HealRate, 1f);
        SetBaseValue(UnitValue.MoveSpeed, 1f);
        SetBaseValue(UnitValue.Range, 0f);
        SetBaseValue(UnitValue.Magnitude, 1f);
        SetBaseValue(UnitValue.AttackSpeed, 1f);
        SetBaseValue(UnitValue.CritChance, 0.01f);
        SetBaseValue(UnitValue.CritRate, 1.2f);

        AddEffect(new BomberEffect(this));
    }

    public override void LevelUp(float mult = 1f)
    {
        GetValueModifier(UnitValue.Health, ModifierData.PER_LEVEL).value += 2f * mult;
        GetValueModifier(UnitValue.HealRate, ModifierData.PER_LEVEL).value += 0.02f * mult;
        GetValueModifier(UnitValue.MoveSpeed, ModifierData.PER_LEVEL).value += 0.03f * mult;
        GetValueModifier(UnitValue.Range, ModifierData.PER_LEVEL).value += 2f * mult;
        GetValueModifier(UnitValue.Magnitude, ModifierData.PER_LEVEL).value += 0.06f * mult;
        GetValueModifier(UnitValue.AttackSpeed, ModifierData.PER_LEVEL).value += 0.06f * mult;
        GetValueModifier(UnitValue.CritChance, ModifierData.PER_LEVEL).value += 0.0025f * mult;
        GetValueModifier(UnitValue.CritRate, ModifierData.PER_LEVEL).value += 0.02f * mult;
        base.LevelUp(mult);
    }
}