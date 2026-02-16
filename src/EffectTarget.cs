using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BrightSpace;

public class EffectTarget
{
    public List<UnitEffect> effectList;

    public EffectTarget()
    {
        effectList = [];
    }

    public UnitEffect Add(UnitEffect unitEffect)
    {
        effectList.Add(unitEffect);
        return unitEffect;
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            var effect = effectList[i];
            effect.Update(gameTime);
            if (effect.isDone)
            {
                effectList.RemoveAt(i);
                i--;
            }
        }
    }
}