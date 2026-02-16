namespace BrightSpace;

public class ScoutEffect : UnitEffect
{
    private PseudoRandom evadeChance;

    public ScoutEffect(Unit target) : base(target)
    {
        name = "Scout Evasion";

        evadeChance = new PseudoRandom(0.1f);

        target.OnDamage += DamageEvent;
    }

    private void DamageEvent(Unit source, ref float damage)
    {
        if (evadeChance.Check())
        {
            damage = 0f;
        }
    }

    protected override void OnEnd()
    {
        base.OnEnd();

        target.OnDamage -= DamageEvent;
    }
}