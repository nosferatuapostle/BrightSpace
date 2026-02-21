using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class TorpedoEffect : UnitEffect
{
    private CountdownTimer cd;
    private bool isReady;
    private Unit attackTarget;

    public TorpedoEffect(Unit target) : base(target)
    {
        name = "Torpedo Double Attack";

        isReady = false;
        cd = new CountdownTimer(0f);

        target.OnAttack += AttackEvent;
    }

    private void AttackEvent(Unit attackTarget)
    {
        isReady = true;
        this.attackTarget = attackTarget;
        cd.Interval = TimeSpan.FromSeconds(target.GetAttackSpeed() * 0.25f);
        cd.Restart();
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        target.OnAttack -= AttackEvent;
    }

    protected override void OnTick(GameTime gameTime)
    {
        cd.Update(gameTime);
        if (cd.State == TimerState.Completed && isReady)
        {
            isReady = false;
            target.CreateProjectile(new ProjectileContext
            {
                owner = target,
                targetPosition = attackTarget.position,
                targetUnit = attackTarget
            });
        }
    }
}