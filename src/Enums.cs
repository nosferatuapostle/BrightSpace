namespace BrightSpace;

public enum GameState
{
    SelectState,
    GamePlayState
}

public enum Faction
{
    None,
    IronCorps,
    Biomantes,
    DuskFleet
}

public enum UnitType
{
    None,
    Asteroid,
    Battlecruiser,
    Bomber,
    Dreadnought,
    Fighter,
    Frigate,
    Scout,
    Support,
    Torpedo
}

public enum UnitValue
{
    None,
    Health,
    HealRate,
    HealthRegen,
    DamageResist,
    Magnitude,
    MoveSpeed,
    Range,
    CritChance,
    CritRate,
    AttackSpeed
}

public enum Tag
{
    None,
    ReflectionBlock,
    SupportBuff
}

public enum ModifierType
{
    None,
    Flat,
    PercentAdd,
    PercentMult
}

public enum ModifierData
{
    PLAYER_BUFF,
    PER_LEVEL,
    SCOUT_EFFECT,
    SUPPORT_EFFECT,
    COMMANDED_DEBUFF
}

public enum WeaponType
{
    None,
    BomberExplosion,
    Bolt,
    Bullet,
    MiniBullet,
    FlameWave,
    Rocket,
    TorpedoRocket,
    Bomb,
    PenetrationBullet,
    AtomBomb,
    ToxicWave,
    LightningBolt
}