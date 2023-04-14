using System;
using UnityEngine;

namespace Main._Scripts.Util
{
	public class PausableTimer : MonoBehaviour
	{
		public bool paused { get; set; }
		public bool running { get; private set; }

		private float _timePassed;
		private float _duration;
		private Action _callback;

		public void StartTimer(float pDurationInSeconds, Action pCallback)
		{
			_timePassed = 0;
			running = true;
			_duration = pDurationInSeconds;
			_callback = pCallback;
		}

		public void Stop()
		{
			running = false;
			paused = false;
			_timePassed = 0;
			_callback = null;
		}
		
		private void Update()
		{
			if (!running) return;
			if (paused) return;

			_timePassed += Time.deltaTime;

			if (_timePassed >= _duration)
			{
				running = false;
				_callback();
			}
		}
	}

	public class PausableElseTimer
	{
		private float _timePassed;
		private float _duration;
		private Action _callback;

		public void StartTimer(float pDurationInSeconds, Action pCallback)
		{
			_timePassed = 0;
			_duration = pDurationInSeconds;
			_callback = pCallback;
		}

		public void Stop()
		{
			_timePassed = 0;
			_callback = null;
		}
		
		public void Update()
		{
			_timePassed += Time.deltaTime;

			if (_timePassed >= _duration)
			{
				if (_callback != null) _callback();
			}
		}
	}
}