namespace BrightSpace;

public delegate void OnLevelUpEvent();

public class Experience
{
    public event OnLevelUpEvent OnLevelUp;

    private float xp;
    private float xpToNextLevel;

    public Experience()
    {
        Reset();
    }

    public void Reset()
    {
        xp = 0f;
        xpToNextLevel = 10f;
    }

    public float GetXP()
    {
        return xp;
    }

    public float GetNextLevelXP()
    {
        return xpToNextLevel;
    }

    public void AddExp(float amount)
    {
        var bonusExp = xpToNextLevel * 0.01f;
        xp += amount + bonusExp;
        while (xp >= xpToNextLevel)
        {
            xp -= xpToNextLevel;
            xpToNextLevel = (int)(xpToNextLevel * 1.2f) + 2;
            OnLevelUp?.Invoke();
        }
    }

    public virtual void AddKillReward(Unit unit, float bonusExp = 0f)
    {
        if (unit.isPlayerTeammate)
        {
            return;
        }
        AddExp(unit.level + 1 + bonusExp);  
    }
}