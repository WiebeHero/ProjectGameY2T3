using TMPro;
using UnityEngine;

namespace Main._Scripts.DragonBlower.BlowStates
{
	// [RequireComponent(typeof())]
	public class BlowMainState : BlowState
	{
		// [SerializeField] private Button _exitButton;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private CountDown _countDown;

		// private void Start()
		// {
		// 	_exitButton.onClick.AddListener(OnExit);
		// }
		//
		// private void OnExit()
		// {
		// 	BlowStateManager.SetState(typeof(BlowFinishState));
		// }

		public override void Initiate()
		{
			base.Initiate();

			
			_countDown.StartCountDown(DragonBlower.instance.gameDuration);
			_countDown.FinishedEvent += () => BlowStateManager.SetState(typeof(BlowFinishState));

			_scoreText.gameObject.SetActive(true);
			DragonBlower.instance.Enable(true);
		}

		public override void Stop()
		{
			DragonBlower.instance.Enable(false);
			base.Stop();
		}

		public override void FixedStateUpdate()
		{
			_scoreText.text = DragonBlower.instance.score.ToString();
		}
	}
}