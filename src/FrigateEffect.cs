using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class FrigateEffect : UnitEffect
{
    private CountdownTimer cd;

    public FrigateEffect(Unit target) : base(target)
    {
        name = "Frigate Reflection";
        target.OnDamage += DamageEvent;

        cd = new CountdownTimer(0.1f);
    }

    private void DamageEvent(Unit source, ref float damage)
    {
        if (cd.State == TimerState.Completed)
        {
            cd.Restart();

            if (source.HasTag(Tag.ReflectionBlock))
            {
                source.RemoveTag(Tag.ReflectionBlock);
                return;
            }

            target.AddTag(Tag.ReflectionBlock);
            source.TakeDamage(target, target.Magnitude(damage * 0.5f));
        }
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        target.OnDamage -= DamageEvent;
    }

    protected override void OnTick(GameTime gameTime)
    {
        cd.Update(gameTime);
    }
}