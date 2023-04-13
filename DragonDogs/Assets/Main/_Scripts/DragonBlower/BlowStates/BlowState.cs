using UnityEngine;

namespace Main._Scripts.DragonBlower.BlowStates
{
	public class BlowState : MonoBehaviour
	{
		[SerializeField] private GameObject _container;

		public virtual void Initiate()
		{
			if (_container != null) _container.SetActive(true);
		}

		public virtual void FixedStateUpdate() {}

		public virtual void Stop()
		{
			if (_container != null) _container.SetActive(false);
		}
	}
}