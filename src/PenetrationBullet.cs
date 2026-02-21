namespace BrightSpace;

public class PenetrationBullet : AnimatedProjectile
{
    public PenetrationBullet(ProjectileContext context) : base(context, 12f, "projectile_07", 9, 12, 8)
    {
        radius = 2f;
        speed = 440f;
    }

    // explosion effect generate explosion projectile
}