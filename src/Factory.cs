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
            UnitType.Torpedo => new Torpedo(faction),
            _ => new Scout(faction)
        };
        unit.PostCreate();

        World.AddUnit(unit);

        return unit;
    }

    public static Projectile CreateProjectile(WeaponType type, ProjectileContext context)
    {
        Projectile projectile = type switch
        {
            WeaponType.BomberExplosion => new BomberExplosion(context),
            WeaponType.Bolt => new Bolt(context),
            WeaponType.Bullet => new Bullet(context),
            WeaponType.MiniBullet => new MiniBullet(context),
            WeaponType.FlameWave => new FlameWave(context),
            WeaponType.Rocket => new Rocket(context),
            WeaponType.TorpedoRocket => new TorpedoRocket(context),
            WeaponType.Bomb => new Bomb(context),
            WeaponType.PenetrationBullet => new PenetrationBullet(context),
            WeaponType.AtomBomb => new AtomBomb(context),
            WeaponType.ToxicWave => new ToxicWave(context),
            WeaponType.LightningBolt => new LightningBolt(context),
            _ => new LightningBolt(context)
        };
        projectile.PostCreate();

        World.AddProjectile(projectile);

        return projectile;
    }
}