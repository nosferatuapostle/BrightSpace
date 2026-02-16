namespace BrightSpace;

public static class Factory
{
    public static Unit CreateUnit(UnitType type, Faction faction)
    {
        Unit unit = type switch
        {
            UnitType.Battlecruiser => new Battlecruiser(faction),
            UnitType.Bomber => new Bomber(faction),
            UnitType.Dreadnought => new Dreadnought(faction),
            UnitType.Fighter => new Fighter(faction),
            UnitType.Frigate => new Frigate(faction),
            UnitType.Scout => new Scout(faction),
            UnitType.Support => new Support(faction),
            _ => new Torpedo(faction)
        };
        unit.Awake();

        World.AddUnit(unit);

        return unit;
    }

    public static Projectile CreateProjectile(WeaponType type, ProjectileContext context)
    {
        Projectile projectile = type switch
        {
            WeaponType.BomberExplosion => new BomberExplosion(context),
            WeaponType.LightningBolt => new LightningBolt(context),
            _ => new AnimatedProjectile(context)
        };
        projectile.Awake();

        World.AddProjectile(projectile);

        return projectile;
    }
}