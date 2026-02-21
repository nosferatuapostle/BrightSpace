using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace BrightSpace;

public abstract class AnimatedProjectile : Projectile
{
    private AnimatedSprite animatedSprite;

    public AnimatedProjectile(ProjectileContext context, float lifeTimeDuration, string texturePath, int width, int height, int frames) : base(context, lifeTimeDuration)
    {
        animatedSprite = Utils.CreateAnimatedSprite(texturePath, width, height, frames);
        sprite = animatedSprite;
    }

    public override void Update(GameTime gameTime)
    {        
        base.Update(gameTime);
        animatedSprite.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(animatedSprite, transform);
    }
}