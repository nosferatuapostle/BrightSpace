using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace BrightSpace;

public class AnimatedProjectile : Projectile
{
    private AnimatedSprite animatedSprite;

    public AnimatedProjectile(ProjectileContext context) : base(context, 20f)
    {
        speed = 200f;
        radius = 2f;

        animatedSprite = Utils.CreateAnimatedSprite("projectile", 9, 9, 5);
        sprite = animatedSprite;
    }

    public override void Update(GameTime gameTime)
    {
        var targetUnit = context.targetUnit;
        if (targetUnit != null && !targetUnit.isDead && context.owner.HostileTo(targetUnit))
        {
            UpdateDirection(targetUnit.position);
        }
        
        base.Update(gameTime);
        animatedSprite.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(animatedSprite, transform);
    }
}