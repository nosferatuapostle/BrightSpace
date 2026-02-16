using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrightSpace;

public class BranchLightning : Lightning
{
    private readonly List<List<Vector2>> branches = [];
    private int maxBranches = 3;

    public float branchChance = 0.3f;
    public float branchLengthFactor = 0.5f;
    
    public override void Create(Vector2 start, Vector2 end)
    {
        base.Create(start, end);
        GenerateBranches();
    }

    private void GenerateBranches()
    {
        branches.Clear();

        for (int i = 1; i < pointList.Count - 1; i++)
        {
            if (Utils.Random.NextSingle() < branchChance && branches.Count < maxBranches)
            {
                var branchStart = pointList[i];
                var direction = pointList[i] - pointList[i - 1];
                direction.Normalize();

                var perp = new Vector2(-direction.Y, direction.X);
                if (Utils.Random.Next(2) == 0)
                {
                    perp *= -1;
                }

                var branchEnd = branchStart + perp * (Vector2.Distance(start, end) * branchLengthFactor);

                var branchPoints = new List<Vector2> { branchStart };
                BuildSegment(branchStart, branchEnd, (branchEnd - branchStart).Length() * displacementFactor, detail, branchPoints);

                branches.Add(branchPoints);
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (IsDone)
        {
            return;
        }

        var baseColor = color * currentAlpha;
        foreach (var branch in branches)
        {
            if (branch.Count < 2)
            {
                continue;
            }

            if (glowEnabled)
            {
                var glowColor = baseColor * glowAlpha;
                for (int i = 0; i < branch.Count - 1; i++)
                {
                    DrawLine(spriteBatch, branch[i], branch[i + 1], thickness * glowScale, glowColor);
                }
            }

            for (int i = 0; i < branch.Count - 1; i++)
            {
                DrawLine(spriteBatch, branch[i], branch[i + 1], thickness, baseColor);
            }
        }
    }
}