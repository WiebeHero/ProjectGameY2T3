using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main._Scripts.DragonBlower.BlowStates
{
	public class BlowFinishState : BlowState
	{
		[SerializeField] private Button _restart;
		[SerializeField] private Button _exit;
		[SerializeField] private TextMeshProUGUI _scoreText;

		private void Start()
		{
			_restart.onClick.AddListener(OnRestart);
		}

		private void OnRestart()
		{
			BlowStateManager.SetState(typeof(BlowStartState));
		}

		private void OnExit()
		{
			Debug.LogWarning("TODO: IMPLEMENT EXITING TO MAIN SCENE");
		}
		
		public override void Stop()
		{
			_scoreText.gameObject.SetActive(false);
			base.Stop();
		}
	}
}