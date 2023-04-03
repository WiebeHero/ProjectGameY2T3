using Main._Scripts;
using Main._Scripts.FlappyDragon.States;
using UnityEngine;
using UnityEngine.UI;

namespace User._Scripts.FlappyDragon.States
{
	public class StartState : CustomState
	{
		[SerializeField] private ObstacleCourse _obstacleCourse;
		[SerializeField] private Dragon _dragon;
		[SerializeField] private GameObject _startWindow;
		[SerializeField] private Button _startButton;

		public override void StateReset() {}

		public override void StateStart()
		{
			_obstacleCourse.NewCourse();
			_obstacleCourse.PauseCourse(true);
			
			FlappyDragon.instance.score = 0;
			_startWindow.SetActive(true);
			_startButton.onClick.AddListener(OnStartClick);
			
			_dragon.EnableGravity(false);
			_dragon.LockInput(true);
			_dragon.ResetPosition();
			
		}
		
		private void OnStartClick()
		{
			FlappyStateManager.SetState(typeof(GameState));
		}

		public override void StateUpdate() {}

		public override void FixedStateUpdate() {}

		public override void Stop()
		{
			_startButton.onClick.RemoveListener(OnStartClick);
			_startWindow.SetActive(false);
		}
	}
}