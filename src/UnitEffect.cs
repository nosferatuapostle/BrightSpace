using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public abstract class UnitEffect
{
    public string name;
    public Unit target;

    public bool isDone;

    public float duration;
    public CountdownTimer timer;

    public UnitEffect(Unit target, float duration = 0f)
    {
        this.target = target;
        this.duration = duration;
        if (duration > 0f)
        {
            timer = new CountdownTimer(duration);
        }
    }

    protected virtual void OnTick(GameTime gameTime) { }

    protected virtual void OnEnd()
    {
        isDone = true;
    }

    public void Update(GameTime gameTime)
    {
        OnTick(gameTime);
        if (timer != null)
        {
            timer.Update(gameTime);
            if (timer.State == TimerState.Completed)
            {
                OnEnd();
            }
        }
    }
}