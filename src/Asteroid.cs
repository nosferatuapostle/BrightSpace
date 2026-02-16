using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace BrightSpace;

public class Asteroid : Unit
{
    public Asteroid()
    {
        faction = Faction.None;
        type = UnitType.Asteroid;

        var tex = Core.Content.Load<Texture2D>($"asteroid_{Utils.Random.Next(0, 1)}");
        sprite = new Sprite(tex)
        {
            Origin = new Vector2(64 / 2f, 64 / 2f)
        };

        level = 0;

        baseSpeed = 0f;
        baseDamage = 0f;
        radius = 20f;
    }

    public override void DeathEvent(Unit dying, Unit killer)
    {
        base.DeathEvent(dying, killer);
        isDestroyed = true;
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, transform);
    }
}