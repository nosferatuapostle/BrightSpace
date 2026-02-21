using System;
using Microsoft.Xna.Framework;

namespace BrightSpace;

public class LightningBolt : Projectile
{
    public LightningBolt(ProjectileContext context) : base(context, 1f)
    {
        speed = 0f;
        radius = 4f;
        position = context.targetPosition;

        World.CreateLightning(2f, context.owner.position, position);
    }

    public override bool OnCollision()
    {
        for (int i = 0; i < World.UnitList.Count; i++)
        {
            var u = World.UnitList[i];
            var owner = context.owner;

            if (u == owner || u.isDead || !owner.HostileTo(u))
            {
                continue;
            }

            if ((u.position - position).Length() < u.radius + radius)
            {
                HitAction(u);
            }
        }
        return true;
    }
}