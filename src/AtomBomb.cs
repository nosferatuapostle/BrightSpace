namespace BrightSpace;

public class AtomBomb : AnimatedProjectile
{
    public AtomBomb(ProjectileContext context) : base(context, 12f, "projectile_08", 8, 8, 8)
    {
        radius = 2f;
        speed = 390f;
    }

    // explosion effect generate explosion projectile
}