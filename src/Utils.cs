using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Graphics;

namespace BrightSpace;

public static class Utils
{
    public static readonly FastRandom Random = new();

    public static AnimatedSprite CreateAnimatedSprite(string texturePath, int width, int height, int frames, string animationName = "base", bool loop = true, float frameTime = 0.1f)
    {
        var texture = Core.Content.Load<Texture2D>(texturePath);
        var atlas = Texture2DAtlas.Create(null, texture, width, height);
        var spriteSheet = new SpriteSheet(null, atlas).DefineAnimation(animationName, frames, loop, frameTime);
        return new AnimatedSprite(spriteSheet, animationName)
        {
            Origin = new Vector2(width / 2f, height / 2f)
        };
    }

    public static AnimatedSprite CreateAnimatedSprite(string texturePath, int width, int height, int[] frames, string animationName = "base", bool loop = true, float frameTime = 0.1f)
    {
        var texture = Core.Content.Load<Texture2D>(texturePath);
        var atlas = Texture2DAtlas.Create(null, texture, width, height);
        var spriteSheet = new SpriteSheet(null, atlas).DefineAnimation(animationName, frames, loop, frameTime);
        return new AnimatedSprite(spriteSheet, animationName)
        {
            Origin = new Vector2(width / 2f, height / 2f)
        };
    }

    public static SpriteSheet DefineAnimation(this SpriteSheet spriteSheet, string name, int frames, bool loop = true, float frameTime = 0.1f)
    {
        var timeSpan = TimeSpan.FromSeconds(frameTime);
        spriteSheet.DefineAnimation(name, builder =>
        {
            builder.IsLooping(loop);
            for (int i = 0; i < frames; i++)
            {
                builder.AddFrame(i, timeSpan);
            }
        });

        return spriteSheet;
    }

    public static SpriteSheet DefineAnimation(this SpriteSheet spriteSheet, string name, int[] frames, bool loop = true, float frameTime = 0.1f)
    {
        var timeSpan = TimeSpan.FromSeconds(frameTime);
        spriteSheet.DefineAnimation(name, builder =>
        {
            builder.IsLooping(loop);
            foreach (var frame in frames)
            {
                builder.AddFrame(frame, timeSpan);
            }
        });

        return spriteSheet;
    }

    public static Vector2 RotateVector(this Vector2 v, float angle)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Vector2(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
    }

    public static float ToAngle(this Vector2 source)
    {
        return MathF.Atan2(source.Y, source.X) + MathHelper.PiOver2;
    }

    public static float ToAngle(this Vector2 source, Vector2 target)
    {
        var x = target.X - source.X;
        var y = target.Y - source.Y;

        if (x == 0 && y == 0)
        {
            return 0f;
        }

        var result = MathF.Atan2(y, x);
        return float.IsNaN(result) ? 0f : result + MathHelper.PiOver2;
    }
}