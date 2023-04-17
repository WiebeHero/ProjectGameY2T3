using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main._Scripts.DragonBlower.BlowStates
{
	public class BlowFinishState : BlowState
	{
		[SerializeField] private Button _restart;
		[SerializeField] private Button _exit;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private AudioSource source;

		private void Start()
		{
			_restart.onClick.AddListener(OnRestart);
			Debug.Log("In finish");
		}

		private void OnRestart()
		{
			BlowStateManager.SetState(typeof(BlowStartState));
		}

		private void OnExit()
		{
			SceneManager.LoadScene("Main");
			Debug.LogWarning("TODO: IMPLEMENT EXITING TO MAIN SCENE");
		}
		
		public override void Stop()
		{
			Debug.Log("Out finish");
			_scoreText.gameObject.SetActive(false);
			base.Stop();
		}
	}
}