using System.Collections.Generic;

namespace BrightSpace;

public class ToxicWave : AnimatedProjectile
{
    private Dictionary<Unit, float> hitDelay;

    public ToxicWave(ProjectileContext context) : base(context, 12f, "projectile_09", 64, 64, 6)
    {
        radius = 24f;
        speed = 300f;

        hitDelay = [];
    }

    public override bool OnCollision()
    {
        var owner = context.owner;
        var currentTime = Time.Total;

        for (int i = 0; i < World.UnitList.Count; i++)
        {
            var u = World.UnitList[i];

            if (u == owner || u.isDead)
            {
                continue;
            }
            
            if ((u.position - position).Length() < u.radius + radius)
            {
                if (owner.isPlayer || u.HostileTo(owner))
                {
                    const float delayTime = 0.5f;
                    if (!hitDelay.TryGetValue(u, out var time) || currentTime >= time)
                    {
                        HitAction(u);
                        hitDelay[u] = currentTime + delayTime;
                    }
                }
            }
        }
        return false;
    }
}