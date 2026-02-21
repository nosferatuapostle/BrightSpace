using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Torpedo : SpaceShip
{
    public Torpedo(Faction faction) : base(faction, 19f)
    {
        name = "Torpedo";
        type = UnitType.Torpedo;

        radius = 20f;

        var size = Data.UNIT_SIZE_64;
        this.size = new Vector2(size);

        if (faction == Faction.IronCorps)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_ironcorps_torpedo", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_ironcorps_torpedo_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            deathSprite = Utils.CreateAnimatedSprite("unit_ironcorps_torpedo_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8], "base", false);
        }
        else if (faction == Faction.Biomantes)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_biomantes_torpedo", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_biomantes_torpedo_engine", size, size, [0, 1, 2, 3, 4, 5, 6]);
            deathSprite = Utils.CreateAnimatedSprite("unit_biomantes_torpedo_destruction", size, size, [0, 1, 2, 3, 4, 5, 6], "base", false);
        }
        else
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_duskfleet_torpedo", size, size, [0]);
            engineSprite = Utils.CreateAnimatedSprite("unit_duskfleet_torpedo_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_duskfleet_torpedo_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15], "base", false);
        }

        baseSpeed = 100f;

        SetBaseValue(UnitValue.HealRate, 1f);
        SetBaseValue(UnitValue.MoveSpeed, 1f);
        SetBaseValue(UnitValue.Range, 0f);
        SetBaseValue(UnitValue.Magnitude, 0.8f);
        SetBaseValue(UnitValue.AttackSpeed, 1f);
        SetBaseValue(UnitValue.CritChance, 0.01f);
        SetBaseValue(UnitValue.CritRate, 1.2f);

        AddEffect(new TorpedoEffect(this));
    }

    public override void LevelUp(float mult = 1f)
    {
        GetValueModifier(UnitValue.Health, ModifierData.PER_LEVEL).value += 2f * mult;
        GetValueModifier(UnitValue.HealRate, ModifierData.PER_LEVEL).value += 0.02f * mult;
        GetValueModifier(UnitValue.MoveSpeed, ModifierData.PER_LEVEL).value += 0.026f * mult;
        GetValueModifier(UnitValue.Range, ModifierData.PER_LEVEL).value += 3f * mult;
        GetValueModifier(UnitValue.Magnitude, ModifierData.PER_LEVEL).value += 0.025f * mult;
        GetValueModifier(UnitValue.AttackSpeed, ModifierData.PER_LEVEL).value += 0.06f * mult;
        GetValueModifier(UnitValue.CritChance, ModifierData.PER_LEVEL).value += 0.0025f * mult;
        GetValueModifier(UnitValue.CritRate, ModifierData.PER_LEVEL).value += 0.02f * mult;
        base.LevelUp();
    }
}