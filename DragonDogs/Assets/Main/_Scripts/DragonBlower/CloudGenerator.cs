using Main._Scripts.Util;
using UnityEngine;

namespace Main._Scripts.DragonBlower
{
	[RequireComponent(typeof(RectTransform))]
	
	[RequireComponent(typeof(PausableTimer))]
	public class CloudGenerator : MonoBehaviour
	{
		[SerializeField] private GameObject _cloudPrefab;
		[SerializeField] private float _spawnDelay;

		private RectTransform _rectTransform;
		private PausableTimer _pausableTimer;

		private Vector2 _xBounds;

		public bool running { get; set; }

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
			_pausableTimer = GetComponent<PausableTimer>();
		}

		public void Pause(bool pPause)
		{
			_pausableTimer.paused = pPause;
			running = pPause;
		}

		public void StartSpawning()
		{
			running = true;
			SpawnCloud();
			_pausableTimer.StartTimer(_spawnDelay, SpawnCloud);
		}

		private void Start()
		{
			Vector3 position = transform.position;
			
			Rect rect = _rectTransform.rect;
			_xBounds.x = position.x - rect.width/4;
			_xBounds.y = position.x + rect.width/4;
		}

		private void SpawnCloud()
		{
			if (!running) return;
			
			Vector3 position = transform.position;
			
			float x = UnityEngine.Random.Range(_xBounds.x, _xBounds.y);
			GameObject cloud = Instantiate(_cloudPrefab, new Vector3(x,position.y,90), Quaternion.identity, _rectTransform);
			cloud.GetComponent<Cloud>().Initialize(this);
			_pausableTimer.StartTimer(_spawnDelay, SpawnCloud);
		}

		public void StopSpawning()
		{
			running = false;
		}
	}
}