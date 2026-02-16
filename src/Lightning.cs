using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrightSpace;

public class Lightning
{
    protected static Texture2D pixel;

    public List<Vector2> pointList = [];

    public Color color;
    public float thickness = 2f;
    public float detail = 2f;
    public float displacementFactor = 0.5f;

    public bool glowEnabled = true;
    public float glowScale = 2.5f;
    public float glowAlpha = 0.25f;

    public Vector2 start;
    public Vector2 end;

    public float lifetime = 0.3f;
    public float timeLeft;
    protected float currentAlpha = 1f;

    public bool IsDone { get { return timeLeft <= 0; } }

    public Lightning(Color color)
    {
        if (pixel == null)
        {
            pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
            pixel.SetData([Color.White]);
        }

        this.color = color;
    }

    public Lightning() : this(Color.White) { }

    public void SetEndpoints(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }

    public virtual void Create(Vector2 start, Vector2 end)
    {
        SetEndpoints(start, end);
        Rebuild();
        timeLeft = lifetime;
        currentAlpha = 1f;
    }

    public virtual void Rebuild()
    {
        pointList.Clear();
        pointList.Add(start);

        var len = Vector2.Distance(start, end);
        var initialDisplacement = Math.Max(detail, len * displacementFactor);
        BuildSegment(start, end, initialDisplacement, detail, pointList);
    }

    protected void BuildSegment(Vector2 a, Vector2 b, float displacement, float detail, List<Vector2> list)
    {
        if (displacement < detail)
        {
            list.Add(b);
            return;
        }

        var mid = (a + b) / 2;
        mid += new Vector2(
            (Utils.Random.NextSingle() - 0.5f) * displacement,
            (Utils.Random.NextSingle() - 0.5f) * displacement
        );

        BuildSegment(a, mid, displacement / 2, detail, list);
        BuildSegment(mid, b, displacement / 2, detail, list);
    }

    public virtual void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.Delta;
            currentAlpha = MathHelper.Clamp(timeLeft / lifetime, 0f, 1f);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (pointList.Count < 2 || IsDone)
            return;

        var baseColor = color * currentAlpha;

        if (glowEnabled)
        {
            var glowColor = baseColor * glowAlpha;
            for (int i = 0; i < pointList.Count - 1; i++)
                DrawLine(spriteBatch, pointList[i], pointList[i + 1], thickness * glowScale, glowColor);
        }

        for (int i = 0; i < pointList.Count - 1; i++)
            DrawLine(spriteBatch, pointList[i], pointList[i + 1], thickness, baseColor);
    }

    protected void DrawLine(SpriteBatch sb, Vector2 a, Vector2 b, float thickness, Color color)
    {
        var edge = b - a;
        var length = edge.Length();
        if (length <= 0.0001f)
            return;

        var angle = (float)Math.Atan2(edge.Y, edge.X);
        var rect = new Rectangle((int)a.X, (int)a.Y, (int)Math.Ceiling(length), (int)Math.Max(1f, thickness));
        sb.Draw(pixel, rect, null, color, angle, new Vector2(0f, 0.5f), SpriteEffects.None, 0f);
    }
}