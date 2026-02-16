namespace BrightSpace;

public static class Time
{
    public static float Total;
    public static float Delta;

    internal static void Update(float deltaTime)
    {
        Total += deltaTime;
        Delta = deltaTime;
    }
}