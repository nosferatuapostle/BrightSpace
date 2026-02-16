using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class SpaceShip : Unit
{
    public AnimatedSprite animatedSprite;
    public AnimatedSprite engineSprite;
    public AnimatedSprite deathSprite;

    private CountdownTimer engineTimer;

    public SpaceShip(Faction faction)
    {
        sprite = animatedSprite;
        this.faction = faction;

        engineTimer = new CountdownTimer(1.2f);

        AddValueModifier(UnitValue.Health, ModifierData.PER_LEVEL, new ValueModifier(0f, ModifierType.Flat));
        AddValueModifier(UnitValue.Magnitude, ModifierData.PER_LEVEL, new ValueModifier(0f, ModifierType.Flat));
        AddValueModifier(UnitValue.SpeedMult, ModifierData.PER_LEVEL, new ValueModifier(0f, ModifierType.Flat));
        AddValueModifier(UnitValue.Range, ModifierData.PER_LEVEL, new ValueModifier(0f, ModifierType.Flat));
        AddValueModifier(UnitValue.AttackSpeed, ModifierData.PER_LEVEL, new ValueModifier(0f, ModifierType.Flat));
    }

    public override void Awake()
    {
        base.Awake();
        engineSprite.IsVisible = false;
        deathSprite.IsVisible = false;
        deathSprite.Controller.Pause();
    }

    public override void DeathEvent(Unit dying, Unit killer)
    {
        base.DeathEvent(dying, killer);

        SetUnitTarget(null);
        MoveStop();

        animatedSprite.IsVisible = false;
        engineSprite.IsVisible = false;

        deathSprite.IsVisible = true;
        deathSprite.Controller.Unpause();

        deathSprite.Controller.OnAnimationEvent += (_, e) =>
        {
            if (e == AnimationEventTrigger.AnimationCompleted)
            {
                isDestroyed = true;
            }
        };
    }

    public override void MoveTo(Vector2 target)
    {
        engineTimer.Pause();
        engineSprite.IsVisible = true;
        base.MoveTo(target);
    }

    public override void MoveStop()
    {
        engineTimer.Restart();
        base.MoveStop();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        animatedSprite.Update(gameTime);
        engineSprite.Update(gameTime);
        deathSprite.Update(gameTime);

        engineTimer.Update(gameTime);
        if (engineSprite.IsVisible && engineTimer.State == TimerState.Completed)
        {
            engineSprite.IsVisible = false;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!isDead)
        {
            spriteBatch.Draw(engineSprite, transform);
            spriteBatch.Draw(animatedSprite, transform);
        }
        else
        {
            spriteBatch.Draw(deathSprite, transform);
        }
    }
}