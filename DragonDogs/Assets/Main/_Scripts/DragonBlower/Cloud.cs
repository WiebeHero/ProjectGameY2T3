using Main._Scripts.Util;
using UnityEngine;

namespace Main._Scripts.DragonBlower
{
	[RequireComponent(typeof(SimpleTimer))]
	public class Cloud : MonoBehaviour
	{
		[SerializeField] private float _speed = 1f;
		[SerializeField] private float _lifeTime = 8;
		
		private CloudGenerator _cloudGenerator;

		private bool _initialized;

		public void Initialize(CloudGenerator pCloudGenerator)
		{
			_cloudGenerator = pCloudGenerator;
			GetComponent<SimpleTimer>().StartTimer(_lifeTime, () => Destroy(gameObject));
			_initialized = true;
		}

		private void FixedUpdate()
		{
			if (!_initialized) return;
			if (!_cloudGenerator.running) return;
			transform.Translate(new Vector3(0, -_speed, 0));
		}
	}
}
