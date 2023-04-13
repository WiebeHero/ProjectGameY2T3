using System.Globalization;
using TMPro;
using UnityEngine;

namespace Main._Scripts.DragonBlower.BlowStates
{
	public class BlowMainState : BlowState
	{
		[SerializeField] private TextMeshProUGUI _scoreText;

		public override void Initiate()
		{
			base.Initiate();

			_scoreText.gameObject.SetActive(true);
			DragonBlower.instance.StartGame();
		}

		public override void FixedStateUpdate()
		{
			_scoreText.text = DragonBlower.instance.distanceTravelled.ToString(CultureInfo.CurrentCulture);
		}
	}
}