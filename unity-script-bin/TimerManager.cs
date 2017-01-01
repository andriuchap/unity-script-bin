using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour {

	private static TimerManager instance;
	private static TimerManager Instance
	{
		get {
			if (instance == null) {
				instance = FindObjectOfType<TimerManager> ();
				if (instance == null) {
					GameObject go = new GameObject ("TimerManager");
					TimerManager manager = go.AddComponent<TimerManager> ();
					instance = manager;
				}
			}
			return instance;
		}
	}

	private class Timer
	{
		public float duration = 0f;
		public float elapsedTime = 0f;
		public UnityAction completeAction = null;
		public bool isLooping = false;
		public bool isPaused = false;

		public bool IsFinished {
			get { return elapsedTime >= duration && !isLooping; }
		}

		public Timer(float duration, bool loop, UnityAction completeAction)
		{
			this.duration = duration;
			isLooping = loop;
			this.completeAction = completeAction;
		}

		public void Tick(float dt)
		{
			if (!isPaused) {
				elapsedTime += dt;
				if (elapsedTime >= duration) {
					completeAction.Invoke ();
					if (isLooping) {
						elapsedTime -= duration;
					}
				}
			}
		}
	}

    private Dictionary<uint, Timer> activeTimers;

    private uint currentHandle = 1;

	void Awake () {
		if (instance != null) {
			if (instance != this) {
				Destroy (gameObject);
			}
		}
        activeTimers = new Dictionary<uint, Timer> ();
	}
	
	void Update () {
        float dt = Time.deltaTime;
        List<uint> toRemove = new List<uint>();
        foreach (KeyValuePair<uint, Timer> entry in activeTimers)
        {
            entry.Value.Tick(dt);
            if (entry.Value.IsFinished)
            {
                toRemove.Add(entry.Key);
            }
        }
        for (int i = 0; i < toRemove.Count; i++)
        {
            activeTimers.Remove(toRemove[i]);
        }
    }

	public static uint StartTimer(float duration, bool loop, UnityAction completeAction)
	{
		Timer newTimer = new Timer (duration, loop, completeAction);
		Instance.activeTimers.Add (Instance.currentHandle, newTimer);
		return Instance.currentHandle++;
	}

	public static bool CancelTimer(uint handle)
	{
		return Instance.activeTimers.Remove (handle);
	}

	public static float ElapsedTime(uint handle)
	{
		Timer timer = null;
		if (Instance.activeTimers.TryGetValue (handle, out timer)) {
			return timer.elapsedTime;
		}
		return 0f;
	}

	public static float TimeRemaining(uint handle)
	{
		Timer timer = null;
		if (Instance.activeTimers.TryGetValue (handle, out timer)) {
			return Mathf.Max(timer.duration - timer.elapsedTime, 0f);
		}
		return 0f;
	}

	public static bool IsLooping(uint handle)
	{
		Timer timer = null;
		if (Instance.activeTimers.TryGetValue (handle, out timer)) {
			return timer.isLooping;
		}
		return false;
	}

	public static bool IsPaused(uint handle)
	{
		Timer timer = null;
		if (Instance.activeTimers.TryGetValue (handle, out timer)) {
			return timer.isPaused;
		}
		return false;
	}

	public static bool PauseTimer(uint handle)
	{
		Timer timer = null;
		if (Instance.activeTimers.TryGetValue (handle, out timer)) {
			timer.isPaused = true;
			return true;
		}
		return false;
	}

	public static bool ResumeTimer(uint handle)
	{
		Timer timer = null;
		if (Instance.activeTimers.TryGetValue (handle, out timer)) {
			timer.isPaused = false;
			return true;
		}
		return false;
	}
}
