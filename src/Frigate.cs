using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Frigate : SpaceShip
{
    public Frigate(Faction faction) : base(faction, 24f)
    {
        name = "Frigate";
        type = UnitType.Frigate;

        radius = 20f;

        var size = Data.UNIT_SIZE_64;
        this.size = new Vector2(size);

        if (faction == Faction.IronCorps)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_ironcorps_frigate", size, size, [0, 1, 2, 3, 4, 5]);
            engineSprite = Utils.CreateAnimatedSprite("unit_ironcorps_frigate_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]);
            deathSprite = Utils.CreateAnimatedSprite("unit_ironcorps_frigate_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8], "base", false);
        }
        else if (faction == Faction.Biomantes)
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_biomantes_frigate", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8]);
            engineSprite = Utils.CreateAnimatedSprite("unit_biomantes_frigate_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_biomantes_frigate_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8], "base", false);
        }
        else
        {
            animatedSprite = Utils.CreateAnimatedSprite("unit_duskfleet_frigate", size, size, [0, 1, 2, 3, 4]);
            engineSprite = Utils.CreateAnimatedSprite("unit_duskfleet_frigate_engine", size, size, [0, 1, 2, 3, 4, 5, 6, 7]);
            deathSprite = Utils.CreateAnimatedSprite("unit_duskfleet_frigate_destruction", size, size, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15], "base", false);
        }

        baseSpeed = 100f;

        SetBaseValue(UnitValue.HealRate, 1f);
        SetBaseValue(UnitValue.MoveSpeed, 1f);
        SetBaseValue(UnitValue.Range, 0f);
        SetBaseValue(UnitValue.Magnitude, 1f);
        SetBaseValue(UnitValue.AttackSpeed, 1f);
        SetBaseValue(UnitValue.CritChance, 0.01f);
        SetBaseValue(UnitValue.CritRate, 1.2f);

        AddEffect(new FrigateEffect(this));
    }

    public override void LevelUp(float mult = 1f)
    {
        GetValueModifier(UnitValue.Health, ModifierData.PER_LEVEL).value += 4f * mult;
        GetValueModifier(UnitValue.HealRate, ModifierData.PER_LEVEL).value += 0.02f * mult;
        GetValueModifier(UnitValue.MoveSpeed, ModifierData.PER_LEVEL).value += 0.022f * mult;
        GetValueModifier(UnitValue.Range, ModifierData.PER_LEVEL).value += 3f * mult;
        GetValueModifier(UnitValue.Magnitude, ModifierData.PER_LEVEL).value += 0.04f * mult;
        GetValueModifier(UnitValue.AttackSpeed, ModifierData.PER_LEVEL).value += 0.04f * mult;
        GetValueModifier(UnitValue.CritChance, ModifierData.PER_LEVEL).value += 0.0025f * mult;
        GetValueModifier(UnitValue.CritRate, ModifierData.PER_LEVEL).value += 0.02f * mult;
        base.LevelUp();
    }
}