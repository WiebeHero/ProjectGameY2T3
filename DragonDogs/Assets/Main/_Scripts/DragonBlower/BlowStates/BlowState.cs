using UnityEngine;

namespace Main._Scripts.DragonBlower.BlowStates
{
	public class BlowState : MonoBehaviour
	{
		[SerializeField] private GameObject _container;

		public virtual void Initiate()
		{
			_container.SetActive(true);
		}

		public virtual void FixedStateUpdate() {}

		public virtual void Stop()
		{
			_container.SetActive(false);
		}
	}
}