using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public abstract class Projectile : BasicObject
{
    public bool isDone;

    public float radius;
    public float speed;

    public ProjectileContext context;

    public Vector2 direction;
    public CountdownTimer lifeTime;

    public Projectile(ProjectileContext context, float lifeTimeDuration)
    {
        isDone = false;

        radius = 4f;
        speed = 200f;

        this.context = context;

        position = context.owner.position;
        UpdateDirection(context.targetPosition);

        lifeTime = new CountdownTimer(lifeTimeDuration);
    }
    
    protected virtual void UpdateDirection(Vector2 target)
    {
        direction = target - position;
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
        }
    }

    public override void Update(GameTime gameTime)
    {
        position += direction * speed * Time.Delta;
        rotation = direction.ToAngle();

        lifeTime.Update(gameTime);
        if (lifeTime.State == TimerState.Completed)
        {
            isDone = true;
        }

        if (WasCollision())
        {
            isDone = true;
        }
    }

    protected virtual void HitAction(Unit target)
    {
        target.TakeDamage(context.owner, context.owner.GetBaseDamageValue());
        var effect = ExplosionEffect();
        if (effect != null)
        {
            World.AddParticleEffect(effect);
        }
    }

    public virtual bool WasCollision()
    {
        for (int i = 0; i < World.UnitList.Count; i++)
        {
            var u = World.UnitList[i];

            if (u.isDead)
            {
                continue;
            }

            var owner = context.owner;

            if ((u.position - position).Length() < u.radius + radius && u != owner)
            {
                if (owner.isPlayer || u.HostileTo(owner))
                {
                    HitAction(u);
                    return true;
                }
            }
        }
        return false;
    }

    protected virtual ParticleEffect ExplosionEffect()
    {
        return null;
    }
}