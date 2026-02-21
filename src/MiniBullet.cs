namespace BrightSpace;

public class MiniBullet : AnimatedProjectile
{
    public MiniBullet(ProjectileContext context) : base(context, 12f, "projectile_02", 4, 16, 4)
    {
        radius = 2f;
        speed = 550f;
    }
}