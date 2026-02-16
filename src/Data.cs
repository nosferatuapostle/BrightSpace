using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Graphics;

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

    public static WeaponData LightningBolt()
    {
        return new WeaponData
        {
            name = "LightningBolt",
            type = WeaponType.LightningBolt,
            damage = 1f,
            attackSpeed = 1f
        };
    }
}

public sealed class WeaponData
{
    public string name;
    public WeaponType type;
    public float damage;
    public float attackSpeed;
}

public sealed class ProjectileContext
{
    public Unit owner;
    public Vector2 targetPosition;
    public Unit targetUnit;
}