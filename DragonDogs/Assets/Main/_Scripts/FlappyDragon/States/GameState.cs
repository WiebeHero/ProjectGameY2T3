using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User._Scripts.FlappyDragon;
using User._Scripts.FlappyDragon.States;

namespace Main._Scripts.FlappyDragon.States
{
	public class GameState : CustomState
	{
		[SerializeField] private Dragon _dragon;
		[SerializeField] private ObstacleCourse _obstacleCourse;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private Button _testEndButton;

		public override void StateReset() {}

		public override void StateStart()
		{
			_dragon.LockInput(false);
			_dragon.DisableCollision(false);
			
			_scoreText.text = User._Scripts.FlappyDragon.FlappyDragon.instance.score.ToString();
			_scoreText.gameObject.SetActive(true);
			
			User._Scripts.FlappyDragon.FlappyDragon.instance.onScoreChanged += OnScoreChanged;
			
			_testEndButton.gameObject.SetActive(true);
			_testEndButton.onClick.AddListener(OnTestEndButtonClicked);

			
			_dragon.EnableGravity(true);
			_obstacleCourse.PauseCourse(false);
		}

		private void OnScoreChanged(int pScore)
		{
			_scoreText.text = pScore.ToString();
		}
		
		private void OnTestEndButtonClicked()
		{
			FlappyStateManager.SetState(typeof(FinishState));
		}

		public override void StateUpdate() {}

		public override void FixedStateUpdate()
		{
			if (_obstacleCourse == null) return;
			if (_obstacleCourse.obstacle == null) return;
			_obstacleCourse.obstacle.Move(_obstacleCourse.scrollingSpeed);
		}

		public override void Stop()
		{
			_obstacleCourse.PauseCourse(true);
			
			_scoreText.gameObject.SetActive(false);
			_testEndButton.gameObject.SetActive(false);
			_testEndButton.onClick.RemoveListener(OnTestEndButtonClicked);
			
			User._Scripts.FlappyDragon.FlappyDragon.instance.onScoreChanged -= OnScoreChanged;
		}
	}
}