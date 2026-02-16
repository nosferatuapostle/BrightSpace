using System;

namespace BrightSpace;

public class PseudoRandom
{
    public float baseChance;
    public float increment;
    public float currentChance;
    public float maxChance;
    public int attempts;

    public PseudoRandom(float baseChance, float incrementMultiplier = 0.5f, float maxChance = 1f)
    {
        this.baseChance = baseChance;
        increment = baseChance * incrementMultiplier;
        this.maxChance = maxChance;
        Reset();
    }

    public bool Check()
    {
        attempts++;
        bool success = Utils.Random.NextSingle() < currentChance;

        if (success)
        {
            Reset();
        }
        else
        {
            currentChance = MathF.Min(currentChance + increment, maxChance);
        }

        return success;
    }
    
    public void Reset()
    {
        currentChance = baseChance;
        attempts = 0;
    }
    
    public void SetBaseChance(float newBaseChance, float incrementMultiplier = 0.5f)
    {
        baseChance = newBaseChance;
        increment = baseChance * incrementMultiplier;
        Reset();
    }
}