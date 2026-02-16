using System.Collections;
using System.Collections.Generic;

namespace BrightSpace;

public class CoroutineManager
{
	class CoroutineImpl : ICoroutine, IPoolable
	{
		public IEnumerator Enumerator;

		public float WaitTimer;

		public bool IsDone;
		public CoroutineImpl WaitForCoroutine;

		public void Stop()
		{
			IsDone = true;
		}

		internal void PrepareForReuse()
		{
			IsDone = false;
		}

		void IPoolable.Reset()
		{
			IsDone = true;
			WaitTimer = 0;
			WaitForCoroutine = null;
			Enumerator = null;
		}
	}

	private bool isInUpdate;

	private readonly List<CoroutineImpl> unblockedCoroutines = [];
	private readonly List<CoroutineImpl> shouldRunNextFrame = [];

	public void StopAllCoroutines()
	{
		foreach (var t in unblockedCoroutines)
		{
			t.IsDone = true;
		}

		foreach (var t in shouldRunNextFrame)
		{
			t.IsDone = true;
		}
	}

	public void ClearAllCoroutines()
	{
		if (isInUpdate)
		{
			throw new System.Exception("Cannot call ClearAllCoroutines() while CoroutineManager is updating.");
		}

		foreach (var t in unblockedCoroutines)
		{
			Pool<CoroutineImpl>.Free(t);
		}

		foreach (var t in shouldRunNextFrame)
		{
			Pool<CoroutineImpl>.Free(t);
		}

		unblockedCoroutines.Clear();
		shouldRunNextFrame.Clear();
	}

	public ICoroutine StartCoroutine(IEnumerator enumerator)
	{
		var coroutine = Pool<CoroutineImpl>.Obtain();
		coroutine.PrepareForReuse();

		coroutine.Enumerator = enumerator;
		var shouldContinueCoroutine = TickCoroutine(coroutine);

		if (!shouldContinueCoroutine)
		{
			return null;
		}

		if (isInUpdate)
		{
			shouldRunNextFrame.Add(coroutine);
		}
		else
		{
			unblockedCoroutines.Add(coroutine);
		}

		return coroutine;
	}

	public void Update()
	{
		isInUpdate = true;
		foreach (var coroutine in unblockedCoroutines)
		{
			if (coroutine.IsDone)
			{
				Pool<CoroutineImpl>.Free(coroutine);
				continue;
			}

			if (coroutine.WaitForCoroutine != null)
			{
				if (coroutine.WaitForCoroutine.IsDone)
				{
					coroutine.WaitForCoroutine = null;
				}
				else
				{
					shouldRunNextFrame.Add(coroutine);
					continue;
				}
			}

			if (coroutine.WaitTimer > 0)
			{
				coroutine.WaitTimer -= Time.Delta;
				shouldRunNextFrame.Add(coroutine);
				continue;
			}

			if (TickCoroutine(coroutine))
			{
				shouldRunNextFrame.Add(coroutine);
			}
		}

		unblockedCoroutines.Clear();
		unblockedCoroutines.AddRange(shouldRunNextFrame);
		shouldRunNextFrame.Clear();

		isInUpdate = false;
	}

	private bool TickCoroutine(CoroutineImpl coroutine)
	{
		if (!coroutine.Enumerator.MoveNext() || coroutine.IsDone)
		{
			Pool<CoroutineImpl>.Free(coroutine);
			return false;
		}

		switch (coroutine.Enumerator.Current)
		{
			case null:
				return true;
			case Core.WaitForSeconds seconds:
				coroutine.WaitTimer = seconds.waitTime;
				return true;
			case IEnumerator enumerator:
				coroutine.WaitForCoroutine = StartCoroutine(enumerator) as CoroutineImpl;
				return true;
		}

		if (coroutine.Enumerator.Current is not CoroutineImpl impl)
		{
			return false;
		}
		coroutine.WaitForCoroutine = impl;

		return true;
	}
}