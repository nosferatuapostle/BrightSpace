using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Bomb : AnimatedProjectile
{
    public Bomb(ProjectileContext context) : base(context, 12f, "projectile_06", 16, 16, 16)
    {
        radius = 2f;
        speed = 380f;
    }

    // explosion effect generate explosion projectile
}