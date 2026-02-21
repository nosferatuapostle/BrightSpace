using Microsoft.Xna.Framework;

namespace BrightSpace;

public class TorpedoRocket : AnimatedProjectile
{
    public TorpedoRocket(ProjectileContext context) : base(context, 12f, "projectile_05", 9, 24, 3)
    {
        radius = 4f;
        speed = 360f;
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