using System.Collections.Generic;

namespace BrightSpace;

public static class Pool<T> where T : new()
{
    private static readonly Queue<T> objectQueue = new(10);

    public static void WarmCache(int cacheCount)
    {
        cacheCount -= objectQueue.Count;
        if (cacheCount > 0)
        {
            for (var i = 0; i < cacheCount; i++)
            {
                objectQueue.Enqueue(new T());
            }
        }
    }

    public static void TrimCache(int cacheCount)
    {
        while (objectQueue.Count > cacheCount)
        {
            objectQueue.Dequeue();
        }
    }

    public static void ClearCache()
    {
        objectQueue.Clear();
    }

    public static T Obtain()
    {
        return objectQueue.Count > 0 ? objectQueue.Dequeue() : new T();
    }

    public static void Free(T obj)
    {
        objectQueue.Enqueue(obj);

        if (obj is IPoolable poolable)
        {
            poolable.Reset();
        }
    }
}

public interface IPoolable
{
    void Reset();
}