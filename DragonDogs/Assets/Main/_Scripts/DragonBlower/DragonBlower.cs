using System;
using DG.Tweening;
using Main._Scripts.DragonBlower.BlowStates;
using Main._Scripts.Util;
using UnityEngine;

namespace Main._Scripts.DragonBlower
{
	[RequireComponent(typeof(MicToSpeed))]
	[RequireComponent(typeof(PausableTimer))]
	public class DragonBlower : MonoBehaviour
	{
		[SerializeField] private CloudGenerator _cloudGenerator;
		
		[SerializeField] private int _gameDuration = 5;
		public int gameDuration => _gameDuration;
		
		private MicToSpeed _micToSpeed;

		public static DragonBlower instance;
		
		public Vector3 startPosition { get; private set; }


		private int _distanceTravelled;
		public int distanceTravelled
		{
			get => _distanceTravelled;
			set
			{
				if (value <= 0) _distanceTravelled = 0;
				else _distanceTravelled = value;
			}
		}
		private float speed => _micToSpeed.speed;


		[SerializeField] private float _forwardY = 1;
		[SerializeField] private float _neutralY = 0;
		[SerializeField] private float _backwardsY = -1;
		[SerializeField] private float _movementDuration;
		[SerializeField] private float _durationAtFatalSpeedAllowed = 200;
		[SerializeField] private float _fatalY = -3.8f;
		[SerializeField] private float _speedToGoForward = 0.1f;
		[SerializeField] private float _speedToGoBackwards = 0.1f;

		[SerializeField] private float _targetYForDying = -7;
		[SerializeField] private float _dyingDuration = 2.0f;
		
		
		[Header("Height sampling")] 
		[SerializeField] private int _samplesToCheck = 3;

		private float[] _heightSamples;
		private int _heightSampleIndex = 0;
		
		
		private bool _playing;
		

		private PausableTimer _pausableTimer;
		

		private enum SpeedState
		{
			Forward,
			Neutral,
			Backwards
		}

		private SpeedState _speedState;


		private void SetSpeedState(SpeedState pSpeedState, bool pSnap = false)
		{
			if (pSpeedState == _speedState) return;

			Transform thisTransform = transform;
			Vector3 currentPos = thisTransform.position;

			switch (pSpeedState)
			{
				case SpeedState.Forward:
					if (pSnap) thisTransform.position = new Vector3(currentPos.x, _forwardY, currentPos.z);
					else thisTransform.DOMoveY(_forwardY, _movementDuration);
					
					break;
				case SpeedState.Neutral:
					if (pSnap) thisTransform.position = new Vector3(currentPos.x, _neutralY, currentPos.z);
					else thisTransform.DOMoveY(_neutralY, _movementDuration);
					break;
				case SpeedState.Backwards:
					if (pSnap) thisTransform.position = new Vector3(currentPos.x, +_backwardsY, currentPos.z);
					else gameObject.transform.DOMoveY(_backwardsY, _movementDuration);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(pSpeedState), pSpeedState, null);
			}

			_speedState = pSpeedState;
		}
		
		private void Awake()
		{
			if (instance == null) instance = this;
			else Destroy(gameObject);

			_heightSamples = new float[_samplesToCheck];
			
			_pausableTimer = GetComponent<PausableTimer>();
			_micToSpeed = GetComponent<MicToSpeed>();
		}

		public void ResetGame()
		{
			distanceTravelled = 0;
			
			Transform thisTransform = transform;
			Vector3 position = thisTransform.position;
			position = new Vector3(position.x, _neutralY, position.z);
			thisTransform.position = position;
		}

		public void StartGame()
		{
			_micToSpeed.StartRecording();
			ResetGame();
			SetSpeedState(SpeedState.Neutral, true);
			_playing = true;
			_cloudGenerator.StartSpawning();
		}

		public void StopGame()
		{
			_micToSpeed.StopRecording();
			_playing = false;
			DOTween.KillAll();
			SetSpeedState(SpeedState.Neutral,true);
			BlowStateManager.SetState(typeof(BlowFinishState));
			_cloudGenerator.StopSpawning();
		}


		private bool _dying;

		private float _averageHeight;

		private void FixedUpdate()
		{
			if (_playing)
			{
				_heightSamples[_heightSampleIndex] = transform.position.y;
				_heightSampleIndex++;
				
				if (_heightSampleIndex >= _samplesToCheck)
				{
					_heightSampleIndex = 0;

					_averageHeight = 0;
					
					for (int i = 0; i < _samplesToCheck; i++) _averageHeight += _heightSamples[i];
						
					_averageHeight /= _samplesToCheck;

					Debug.Log(_averageHeight);
				
					//Start death timer or if outside of range pause it, it should reset when it goes back in range
					if (!_dying && _averageHeight <= _fatalY)
					{
						_dying = true;
						_pausableTimer.StartTimer(_durationAtFatalSpeedAllowed, () =>
						{
							_dying = false;
							Die();
						});
					}
					else if (_dying && _averageHeight > _fatalY)
					{
						_pausableTimer.Stop();
						_dying = false;
					}

					if (speed >= _speedToGoForward) SetSpeedState(SpeedState.Forward);
					else if (speed <= _speedToGoBackwards) SetSpeedState(SpeedState.Backwards);
					else SetSpeedState(SpeedState.Neutral);
				
					distanceTravelled += (int)_micToSpeed.speed;
				}
			}
		}

		private void Die()
		{
			_playing = false;
			transform.DOMoveY(_targetYForDying, _dyingDuration).OnComplete(StopGame);
		}


		private void Start()
		{
			startPosition = transform.position;
			BlowStateManager.SetState(typeof(BlowStartState));
		}
	}
}