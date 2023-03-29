using System;
using System.Collections;
using UnityEngine;

namespace User._Scripts.Util
{
	/// <summary>
	/// Simple timer with callback
	/// </summary>
	public class SimpleTimer : MonoBehaviour
	{
		/// <summary>
		/// Starts the timer
		/// </summary>
		/// <param name="pDurationSeconds"> duration in seconds </param>
		/// <param name="pOnEnd"> function that is called on timer end</param>
		public void StartTimer(float pDurationSeconds, Action pOnEnd)
		{
			StartCoroutine(Coroutine());

			IEnumerator Coroutine()
			{
				yield return new WaitForSeconds(pDurationSeconds);
				pOnEnd();
			}
		}
	}
}