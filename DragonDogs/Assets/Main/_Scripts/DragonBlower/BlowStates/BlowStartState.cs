using UnityEngine;
using UnityEngine.UI;

namespace Main._Scripts.DragonBlower.BlowStates
{
	public class BlowStartState : BlowState
	{
		[SerializeField] private Button _startButton;

		private void Start()
		{
			_startButton.onClick.AddListener(OnButtonClick);
		}

		private void OnButtonClick()
		{
			BlowStateManager.SetState(typeof(BlowMainState));
		}
	}
}