using System;
using UnityEngine;

namespace Main._Scripts.FlappyDragon
{
	public class FlappyDragon : MonoBehaviour
	{
		public Action<int> onScoreChanged;

		[SerializeField] private Vector3 _startPosition;
		public Vector3 startPosition => _startPosition;
		
		
		public static FlappyDragon instance;

		private int _score;

		public int score
		{
			get => _score;
			set
			{
				_score = value;
				onScoreChanged?.Invoke(_score);
			}
		}

		private void Awake()
		{

			if (instance == null) instance = this;
			else Destroy(gameObject);
		}
		
		private void Start()
		{
			FlappyStateManager.SetState(typeof(States.StartState));
		}
	}
}