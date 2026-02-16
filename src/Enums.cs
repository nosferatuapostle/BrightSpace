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
    Magnitude,
    SpeedMult,
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
    PLAYER_HEALTH,
    PLAYER_DAMAGE,
    PLAYER_SPEED,
    PER_LEVEL,
    SCOUT_EFFECT,
    SUPPORT_EFFECT
}

public enum WeaponType
{
    None,
    BomberExplosion,
    LightningBolt
}