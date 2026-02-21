using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class BattlecruiserEffect : UnitEffect
{
    private float auraDamage;
    private CountdownTimer cd;

    public BattlecruiserEffect(Unit target) : base(target)
    {
        name = "Battlecruiser Aura";

        auraDamage = 3f;
        cd = new CountdownTimer(1.2f);

        target.OnDeath += DeathEvent;
    }

    private void DeathEvent(Unit dying, Unit killer)
    {
        OnEnd();
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        target.OnDeath -= DeathEvent;
    }

    protected override void OnTick(GameTime gameTime)
    {
        cd.Update(gameTime);
        if (cd.State != TimerState.Completed)
        {
            return;
        }

        foreach (var u in World.UnitList)
        {
            if (u == target || !target.HostileTo(u))
            {
                continue;
            }

            var distance = (u.position - target.position).Length();
            if (distance < 480f)
            {
                cd.Restart();
                u.TakeDamage(target, target.Magnitude(auraDamage));
            }
        }
    }
}