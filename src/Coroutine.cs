using System.Collections;

namespace BrightSpace;

public interface ICoroutine
{
    void Stop();
}

public static class Coroutine
{
    public static readonly CoroutineManager Manager = new();

    public static void Start(IEnumerator coroutine)
    {
        Manager.StartCoroutine(coroutine);
    }

    public static object WaitForSeconds(float seconds)
    {
        return Core.WaitForSeconds.waiter.Wait(seconds);
    }
}