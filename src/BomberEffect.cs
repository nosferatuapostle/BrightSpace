using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class BomberEffect : UnitEffect
{
    private bool isReady;
    private CountdownTimer cd;

    public BomberEffect(Unit target) : base(target)
    {
        name = "Bomber Extra Bomb";

        isReady = false;
        cd = new CountdownTimer(0.1f);

        target.OnHit += HitEvent;
    }

    private void HitEvent(Unit victim, ref float damage)
    {
        if (isReady)
        {
            isReady = false;
            cd.Restart();
            Factory.CreateProjectile(WeaponType.BomberExplosion, new ProjectileContext
            {
                owner = target,
                targetUnit = victim,
                targetPosition = victim.position
            });
        }
    }

    protected override void OnEnd()
    {
        base.OnEnd();

        target.OnHit -= HitEvent;
    }

    protected override void OnTick(GameTime gameTime)
    {
        cd.Update(gameTime);
        if (cd.State == TimerState.Completed)
        {
            isReady = true;
        }
    }
}