using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Bolt : AnimatedProjectile
{
    public Bolt(ProjectileContext context) : base(context, 12f, "projectile_00", 9, 9, 5)
    {
        radius = 2f;
        speed = 460f;
    }

    public override void Update(GameTime gameTime)
    {
        var targetUnit = context.targetUnit;
        if (targetUnit != null && !targetUnit.isDead && context.owner.HostileTo(targetUnit))
        {
            UpdateDirection(targetUnit.position);
        }
        base.Update(gameTime);
    }
}