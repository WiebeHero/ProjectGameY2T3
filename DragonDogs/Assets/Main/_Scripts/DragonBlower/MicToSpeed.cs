using System.Linq;
using UnityEngine;

namespace Main._Scripts.DragonBlower
{
	[RequireComponent(typeof(AudioSource))]
	public class MicToSpeed : MonoBehaviour
	{
		[Header("Loudness calculation")] 
		[SerializeField] private int _sampleWindow = 64;
		
		[Header("Frame sampling")]
		[SerializeField] private float _loudnessSensibility = 100;
		[SerializeField] private float _threshold = 0.1f;
		[SerializeField] private int _sampleFrameAmount = 3;


		[Header("Speed limits")] 
		[SerializeField] private float _maxSpeed = 0.5f;
		[SerializeField] private float _minSpeed = -0.5f;
		
		private float[] _frameSamples;
		private int _frameSampleCounter = 1;

		private AudioClip _micClip;
		private bool _recording;
		
		public float speed { get; private set; }

		private void Awake()
		{
			_frameSamples = new float[_sampleFrameAmount];
		}

		private void Start()
		{
			StartRecording();
		}

		public void Enable(bool pEnable)
		{
			_recording = pEnable;

			if (pEnable) StartRecording();
			else StopRecording();
		}

		private void FixedUpdate()
		{
			if (!_recording) return;
			
			if (_frameSampleCounter >= _sampleFrameAmount)
			{
				float total = _frameSamples.Sum();
				total /= _sampleFrameAmount;
				// transform.position += Vector3.Lerp(new Vector3(), new Vector3(0,1,0), total);
				
				speed = Mathf.Lerp(_minSpeed, _maxSpeed, total);
				
				_frameSampleCounter = 1;
			}
			
			
			float loudness = GetLoudnessFromMicrophoneClip() * _loudnessSensibility;
			if (loudness < _threshold) loudness = 0;
			_frameSamples[_frameSampleCounter - 1] = loudness;
			_frameSampleCounter++;
		}
		
		/// <summary>
		/// Uses default microphone to record an audio clip
		/// </summary>
		public void StartRecording()
		{
			_recording = true;
			_micClip = Microphone.Start(null, true, 20, AudioSettings.outputSampleRate);
		}

		/// <summary>
		/// Stops the default device
		/// </summary>
		public void StopRecording()
		{
			_recording = false;
			Microphone.End(null);
			speed = 0;
		}
		
		public float GetLoudnessFromMicrophoneClip()
		{
			return GetLoudnessFromAudioClip(Microphone.GetPosition(null), _micClip);
		}

		public float GetLoudnessFromAudioClip(int pClipPosition, AudioClip pClip)
		{
			int startClipPosition = pClipPosition - _sampleWindow;

			if (startClipPosition < 0) return 0;

			float[] waveData = new float[_sampleWindow];
			pClip.GetData(waveData, startClipPosition);

			float totalLoudness = 0;

			for (int i = 0; i < _sampleWindow; i++)
			{
				totalLoudness += Mathf.Abs(waveData[i]);
			}
			
			return totalLoudness / _sampleWindow;
		}
	}
}