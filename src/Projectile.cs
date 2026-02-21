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

    protected Vector2 direction;
    protected CountdownTimer lifeTime;

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

        if (OnCollision())
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

    public virtual bool OnCollision()
    {
        var owner = context.owner;

        for (int i = 0; i < World.UnitList.Count; i++)
        {
            var u = World.UnitList[i];

            if (u == owner || u.isDead || !owner.HostileTo(u))
            {
                continue;
            }

            if ((u.position - position).Length() < u.radius + radius && u != owner)
            {
                HitAction(u);
                return true;
            }
        }
        return false;
    }

    protected virtual ParticleEffect ExplosionEffect()
    {
        return null;
    }
}