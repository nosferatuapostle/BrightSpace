using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BrightSpace;

public class MoveMarker
{
    private class Marker
    {
        public Vector2 position;
        public Color color;
        public float timeLeft;
    }

    private static List<Marker> moveMarkerList = [];

    public static void Add(Color color = default)
    {
        var marker = new Marker
        {
            position = World.MousePosition,
            color = color,
            timeLeft = 0.8f
        };

        moveMarkerList.Add(marker);
    }

    public void Update(GameTime gameTime)
    {
        for (int i = moveMarkerList.Count - 1; i >= 0; i--)
        {
            moveMarkerList[i].timeLeft -= Time.Delta;
            if (moveMarkerList[i].timeLeft <= 0f)
            {
                moveMarkerList.RemoveAt(i);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var m in moveMarkerList)
        {
            var t = m.timeLeft / 0.8f;
            var scale = MathHelper.Lerp(1f, 0.5f, 1f - t);
            spriteBatch.DrawCircle(m.position, 4 * scale, 64, m.color * t);
        }
    }
}