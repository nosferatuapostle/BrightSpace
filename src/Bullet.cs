namespace BrightSpace;

public class Bullet : AnimatedProjectile
{
    public Bullet(ProjectileContext context) : base(context, 12f, "projectile_01", 8, 16, 4)
    {
        radius = 2f;
        speed = 500f;
    }
}