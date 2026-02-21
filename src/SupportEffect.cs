using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class SupportEffect : UnitEffect
{
    private CountdownTimer cd;

    public SupportEffect(Unit target) : base(target)
    {
        name = "Support Aura";

        cd = new CountdownTimer(1.2f);

        AddSupportBuff(target);

        target.OnDeath += DeathEvent;
    }

    private void DeathEvent(Unit dying, Unit killer)
    {
        OnEnd();
    }

    private static void AddSupportBuff(Unit unit)
    {
        unit.AddValueModifier(UnitValue.Magnitude, ModifierData.SUPPORT_EFFECT, new ValueModifier(0.1f, ModifierType.Flat));
        unit.AddValueModifier(UnitValue.MoveSpeed, ModifierData.SUPPORT_EFFECT, new ValueModifier(0.1f, ModifierType.Flat));
    }
    
    private static void RemoveSupportBuff(Unit unit)
    {
        unit.RemoveValueModifier(UnitValue.Magnitude, ModifierData.SUPPORT_EFFECT);
        unit.RemoveValueModifier(UnitValue.MoveSpeed, ModifierData.SUPPORT_EFFECT);
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        RemoveSupportBuff(target);
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
            if (u == target || target.HostileTo(u) || u.HasTag(Tag.SupportBuff))
            {
                continue;
            }

            var distance = (u.position - target.position).Length();
            if (distance < 660f)
            {
                cd.Restart();
                u.AddEffect(new TempEffect(u));
            }
        }
    }

    private class TempEffect : UnitEffect
    {
        public TempEffect(Unit target) : base(target, 5f)
        {
            target.AddTag(Tag.SupportBuff);
            AddSupportBuff(target);
        }

        protected override void OnEnd()
        {
            base.OnEnd();

            target.RemoveTag(Tag.SupportBuff);
            RemoveSupportBuff(target);
        }
    }
}