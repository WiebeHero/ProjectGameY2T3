using System;
using System.Collections.Generic;
using Main._Scripts.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main._Scripts.FlappyDragon
{
	[RequireComponent(typeof(PausableTimer))]
	public class ObstacleCourse : MonoBehaviour
	{
		private enum ObstacleType
		{
			Models,
			CustomMesh
		}
		
		[SerializeField] private ObstacleType _obstacleType;
		[SerializeField] private List<GameObject> _modelPrefabs;
		
		
		[Header("Limits")]
		[SerializeField] private GameObject _topLimitObject;
		[SerializeField] private GameObject _bottomLimitObject;
		[SerializeField] private float _openingHeight = 10;
		public float openingHeight => _openingHeight;
		
		
		[Header("Visual properties")]
		[SerializeField] private Material _obstacleMaterial;
		[SerializeField] private float _obstacleGirth = 1;
		[SerializeField] private float _obstacleDepth = 1;
		
		
		[Header("Stage timing")]
		[SerializeField] private float _scrollingSpeed = 10;
		public float scrollingSpeed => _scrollingSpeed;
		[SerializeField] private float _obstacleSpacingSeconds = 10;
		
		private float _topLimit;
		private float _bottomLimit;

		public Obstacle obstacle { get; private set; }
		private GameObject _bottomObstacle;
		private GameObject _topObstacle;
		private GameObject _pointZone;

		private PausableTimer _pausableTimer;

		private void Awake()
		{
			_pausableTimer = GetComponent<PausableTimer>();
			_topLimit = _topLimitObject.transform.position.y;
			_bottomLimit = _bottomLimitObject.transform.position.y;
		}

		public void NewCourse()
		{
			switch(_obstacleType)
			{
				case ObstacleType.Models:
					GenerateFromModels();
					break;
				case ObstacleType.CustomMesh:
					GenerateMesh();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void PauseCourse(bool pPaused)
		{
			_pausableTimer.paused = pPaused;
		}

		private void GenerateFromModels()
		{
			if (_topObstacle != null) Destroy(_topObstacle);
			if (_bottomObstacle != null) Destroy(_bottomObstacle);
			if (_pointZone != null) Destroy(_pointZone);
			if (obstacle != null ) Destroy(obstacle.gameObject);
			
			GameObject selectedTop = _modelPrefabs[Random.Range(0, _modelPrefabs.Count)];
			GameObject selectedBottom = _modelPrefabs[Random.Range(0, _modelPrefabs.Count)];
			
			GameObject obstacleObject = new("Obstacle");
			obstacleObject.transform.parent = transform;
			obstacleObject.transform.position = Vector3.zero;

			_topObstacle = Instantiate(selectedTop);
			// _topObstacle.AddComponent<BoxCollider>();
			_bottomObstacle = Instantiate(selectedBottom);
			// _bottomObstacle.AddComponent<BoxCollider>();


			
			
			float openingTopY = Random.Range(_bottomLimit + _openingHeight, _topLimit);
			float openingBottomY = openingTopY - _openingHeight;

			Debug.Log($"{openingTopY} and {openingBottomY}");
			
			
			float topObstacleHeight = _topObstacle.GetComponentInChildren<MeshRenderer>().bounds.size.y;
			float bottomObstacleHeight = _bottomObstacle.GetComponentInChildren<MeshRenderer>().bounds.size.y;

			Vector3 topLimitPos = _topLimitObject.transform.position;
			Vector3 botLimitPos = _bottomLimitObject.transform.position;
			
			
			_topObstacle.transform.position = new Vector3(topLimitPos.x, openingTopY + topObstacleHeight / 2, topLimitPos.z);
			
			
			_bottomObstacle.transform.position = new Vector3(botLimitPos.x, openingBottomY - bottomObstacleHeight / 2, botLimitPos.z);
			
			_topObstacle.transform.parent = obstacleObject.transform;
			_bottomObstacle.transform.parent = obstacleObject.transform;

			
			obstacle = obstacleObject.AddComponent<Obstacle>();
			_topObstacle.transform.Rotate(180,0,0);

			_pausableTimer.StartTimer(_obstacleSpacingSeconds, GenerateFromModels);

		}

		private void GenerateMesh()
		{
			if (_topObstacle != null) Destroy(_topObstacle);
			if (_bottomObstacle != null) Destroy(_bottomObstacle);
			if (_pointZone != null) Destroy(_pointZone);
			if (obstacle != null ) Destroy(obstacle.gameObject);

			GameObject obstacleObject = new("Obstacle");
			obstacleObject.transform.parent = transform;
			obstacle = obstacleObject.AddComponent<Obstacle>();
			
			_topObstacle = new GameObject("Top");
			_topObstacle.transform.parent = obstacle.transform;
			_topObstacle.AddComponent<MeshFilter>();
			_topObstacle.AddComponent<MeshRenderer>();
			_topObstacle.AddComponent<BoxCollider>();

			_bottomObstacle = new GameObject("Bottom");
			_bottomObstacle.transform.parent = obstacle.transform;
			_bottomObstacle.AddComponent<MeshFilter>();
			_bottomObstacle.AddComponent<MeshRenderer>();
			_bottomObstacle.AddComponent<BoxCollider>();
			
			obstacle.transform.position = Vector3.zero;

			float openingTopY = Random.Range(_bottomLimit + _openingHeight, _topLimit);
			
			CreateTopObstacle(openingTopY);
			CreateBottomObstacle(openingTopY - _openingHeight);

			_pausableTimer.StartTimer(_obstacleSpacingSeconds, GenerateMesh);
		}

		private void CreateTopObstacle(float pOpeningTopY)
		{
			float height = Math.Abs(_topLimit - pOpeningTopY);
			Mesh mesh = CubeMeshGenerator.MakeCube(_obstacleGirth, height, _obstacleDepth); 
			
			AddMeshStuff(_topObstacle, mesh);

			Vector3 newPosition = _topLimitObject.transform.position;
			newPosition.y = _topLimit - height / 2.0f;
			_topObstacle.transform.position = newPosition;
		}

		private void CreateBottomObstacle(float pOpeningBottomY)
		{
			float height = Math.Abs(_bottomLimit - pOpeningBottomY);
			Mesh mesh = CubeMeshGenerator.MakeCube(_obstacleGirth, height, _obstacleDepth); 
			
			AddMeshStuff(_bottomObstacle, mesh);
			
			Vector3 newPosition = _bottomLimitObject.transform.position;
			newPosition.y = _bottomLimit + height / 2.0f;
			_bottomObstacle.transform.position = newPosition;
		}
		
		private void AddMeshStuff(GameObject pGameObject, Mesh pMesh)
		{
			MeshFilter meshFilter = pGameObject.GetComponent<MeshFilter>();
			meshFilter.mesh = pMesh;
			
			pGameObject.GetComponent<MeshRenderer>().material = _obstacleMaterial;

			BoxCollider boxCollider = pGameObject.GetComponent<BoxCollider>();
			boxCollider.center = pMesh.bounds.center;
			boxCollider.size = pMesh.bounds.size;
			boxCollider.bounds.SetMinMax(pMesh.bounds.min, pMesh.bounds.max);
		}
	}
}
