using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Rocket : AnimatedProjectile
{
    public Rocket(ProjectileContext context) : base(context, 12f, "projectile_04", 9, 16, 4)
    {
        radius = 4f;
        speed = 260f;
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

    // explosion effect generate explosion projectile
}