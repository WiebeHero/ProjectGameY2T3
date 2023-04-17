using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main._Scripts.FlappyDragon.States
{
	public class FinishState : CustomState
	{
		[SerializeField] private GameObject _finishWindow;
		[SerializeField] private Button _restartButton;
		[SerializeField] private Button _exitButton;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private Dragon _dragon;
		[SerializeField] private AudioSource _gameOver;

		private bool started;
		
		public override void StateReset() {}
		
		public override void StateStart()
		{
			_gameOver.Play();
			_dragon.LockInput(true);
			_dragon.DisableCollision(true);
			_dragon.ResetVelocity();
			
			_finishWindow.SetActive(true);
			_restartButton.onClick.AddListener(OnRestartButtonClicked);
			_exitButton.onClick.AddListener(OnExitButtonClicked);
			_scoreText.text = FlappyDragon.instance.score.ToString();
		}
		
		private void OnRestartButtonClicked()
		{
			FlappyStateManager.SetState(typeof(StartState));
		}

		private void OnExitButtonClicked()
		{
			Debug.LogWarning("Exiting, to be implemented later");
		}
		
		public override void StateUpdate() {}
		public override void FixedStateUpdate() {}

		public override void Stop()
		{
			
			_gameOver.Stop();
			_finishWindow.SetActive(false);
			_restartButton.onClick.RemoveListener(OnRestartButtonClicked);
			_exitButton.onClick.RemoveListener(OnExitButtonClicked);
		}
	}
}