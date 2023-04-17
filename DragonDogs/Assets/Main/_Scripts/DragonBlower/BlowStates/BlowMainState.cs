using System.Globalization;
using TMPro;
using UnityEngine;

namespace Main._Scripts.DragonBlower.BlowStates
{
	public class BlowMainState : BlowState
	{
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private AudioSource audioGameSource, gameOverSource;

		public override void Initiate()
		{
			base.Initiate();
			
			_scoreText.gameObject.SetActive(true);
			audioGameSource.Play();
			DragonBlower.instance.StartGame();
		}

		public override void Stop()
		{
			audioGameSource.Stop();
			gameOverSource.Play();
			base.Stop();
		}

		public override void FixedStateUpdate()
		{
			_scoreText.text = DragonBlower.instance.distanceTravelled.ToString(CultureInfo.CurrentCulture);
		}
	}
}