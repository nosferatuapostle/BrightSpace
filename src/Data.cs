using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace BrightSpace;

public static class Data
{
    public const int UNIT_SIZE_64 = 64;
    public const int UNIT_SIZE_128 = 128;

    public static BitmapFont Font14 = BitmapFont.FromFile(Core.GraphicsDevice, "res\\font_segou_ui_light_14.fnt");
    public static BitmapFont Font32 = BitmapFont.FromFile(Core.GraphicsDevice, "res\\font_segou_ui_light_32.fnt");

    public static Texture2D Pixel;

    static Data()
    {
        var tex = new Texture2D(Core.GraphicsDevice, 1, 1);
        tex.SetData([Color.White]);
        Pixel = tex;
    }

    public static WeaponData Bolt()
    {
        return new WeaponData
        {
            name = "Bolt",
            type = WeaponType.Bolt,
            damage = 3f,
            attackSpeed = 1.8f,
            range = 350f
        };
    }

    public static WeaponData Bullet()
    {
        return new WeaponData
        {
            name = "Bullet",
            type = WeaponType.Bullet,
            damage = 2f,
            attackSpeed = 0.8f,
            range = 340f
        };
    }

    public static WeaponData MiniBullet()
    {
        return new WeaponData
        {
            name = "Mini Bullet",
            type = WeaponType.MiniBullet,
            damage = 1f,
            attackSpeed = 0.6f,
            range = 360f
        };
    }

    public static WeaponData FlameWave()
    {
        return new WeaponData
        {
            name = "Flame Wave",
            type = WeaponType.FlameWave,
            damage = 5f,
            attackSpeed = 3f,
            range = 320f
        };
    }

    public static WeaponData Rocket()
    {
        return new WeaponData
        {
            name = "Rocket",
            type = WeaponType.Rocket,
            damage = 6f,
            attackSpeed = 3.6f,
            range = 390f
        };
    }

    public static WeaponData TorpedoRocket()
    {
        return new WeaponData
        {
            name = "Torpedo",
            type = WeaponType.TorpedoRocket,
            damage = 5f,
            attackSpeed = 3.5f,
            range = 400f
        };
    }

    public static WeaponData Bomb()
    {
        return new WeaponData
        {
            name = "Bomb",
            type = WeaponType.Bomb,
            damage = 3f,
            attackSpeed = 1.8f,
            range = 380f
        };
    }

    public static WeaponData PenetrationBullet()
    {
        return new WeaponData
        {
            name = "Penetration Bullet",
            type = WeaponType.PenetrationBullet,
            damage = 4f,
            attackSpeed = 1.4f,
            range = 370f
        };
    }

    public static WeaponData AtomBomb()
    {
        return new WeaponData
        {
            name = "Atom Bomb",
            type = WeaponType.AtomBomb,
            damage = 4f,
            attackSpeed = 2f,
            range = 340f
        };
    }

    public static WeaponData ToxicWave()
    {
        return new WeaponData
        {
            name = "Toxic Wave",
            type = WeaponType.ToxicWave,
            damage = 5f,
            attackSpeed = 3f,
            range = 320f
        };
    }


    public static WeaponData LightningBolt()
    {
        return new WeaponData
        {
            name = "Lightning Bolt",
            type = WeaponType.LightningBolt,
            damage = 4f, // 8
            attackSpeed = 2f, // 4.5
            range = 420f
        };
    }
}

public sealed class WeaponData
{
    public string name;
    public WeaponType type;
    public float damage;
    public float attackSpeed;
    public float range;
}

public sealed class ProjectileContext
{
    public Unit owner;
    public Vector2 targetPosition;
    public Unit targetUnit;
}