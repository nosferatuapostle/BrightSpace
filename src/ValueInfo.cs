using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BrightSpace;

public class ValueInfo
{
    public float baseValue;
    public float minValue;
    public float maxValue;
    private Dictionary<ModifierData, ValueModifier> modifierList;

    public ValueInfo(float baseValue, float minValue, float maxValue)
    {
        this.baseValue = baseValue;
        this.minValue = minValue;
        this.maxValue = maxValue;

        modifierList = [];
    }

    public float value
    {
        get
        {
            var endValue = baseValue;
            var percentAdd = 0f;
            var percentMult = 1f;

            foreach (var mod in modifierList.Values)
            {
                switch (mod.type)
                {
                    case ModifierType.Flat:
                        endValue += mod.value;
                        break;
                    case ModifierType.PercentAdd:
                        percentAdd += mod.value;
                        break;
                    case ModifierType.PercentMult:
                        percentMult *= 1 + mod.value;
                        break;
                }
            }

            endValue *= 1 + percentAdd;
            endValue *= percentMult;

            return MathHelper.Clamp(endValue, minValue, maxValue);
        }
    }

    public void AddModifier(ModifierData data, ValueModifier modifier)
    {
        modifierList[data] = modifier;
    }

    public ValueModifier GetModifier(ModifierData data)
    {
        if (modifierList.TryGetValue(data, out var modifier))
        {
            return modifier;
        }

        return null;
    }

    public bool HasModifer(ModifierData data)
    {
        if (modifierList.TryGetValue(data, out var _))
        {
            return true;
        }

        return false;
    }

    public void RemoveModifier(ModifierData data)
    {
        modifierList.Remove(data);
    }
}