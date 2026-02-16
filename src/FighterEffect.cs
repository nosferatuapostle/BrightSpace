namespace BrightSpace;

public class FighterEffect : UnitEffect
{
    private float amount;

    public FighterEffect(Unit target) : base(target)
    {
        name = "Fighter Damage Booster";
        amount = 0.1f;

        target.AddValueModifier(UnitValue.Magnitude, ModifierData.SCOUT_EFFECT, new ValueModifier(0f, ModifierType.Flat));
        target.OnHit += HitEvent;
    }
    
    private void HitEvent(Unit source, ref float damage)
    {
        target.AddEffect(new TempEffect(target, amount));
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        target.OnHit -= HitEvent;
        target.RemoveValueModifier(UnitValue.Magnitude, ModifierData.SCOUT_EFFECT);
    }

    private class TempEffect : UnitEffect
    {
        private float amount;
 
        public TempEffect(Unit target, float amount) : base(target, 5f)
        {
            this.amount = amount;
            target.GetValueModifier(UnitValue.Magnitude, ModifierData.SCOUT_EFFECT).value += amount;
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            target.GetValueModifier(UnitValue.Magnitude, ModifierData.SCOUT_EFFECT).value -= amount;
        }
    }
}