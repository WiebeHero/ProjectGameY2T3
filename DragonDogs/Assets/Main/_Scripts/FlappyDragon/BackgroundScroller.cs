using System;
using System.Collections.Generic;
using Main._Scripts.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main._Scripts.FlappyDragon
{
	
	
	public class BackgroundScroller : MonoBehaviour
	{
		private class DestroyLayerInfo
		{
			public PausableElseTimer timer;
			public GameObject container;
			public bool offsetHappened = false;
		}
		
		[SerializeField] private List<BackgroundLayer> _layers;
		private readonly Dictionary<BackgroundLayer, DestroyLayerInfo> _destroyInfo = new();

		public Canvas canvas { get; private set; }
		

		public bool running { get; private set; } = false;

		public void StartGenerating()
		{
			for (int i = 1; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
			for (int i = 0; i < _layers.Count; i++)
			{
				BackgroundLayer layer = _layers[i];
				layer.z = i;
				HandleLayer(layer);
			}

			running = true;
		}

		public void StopGenerating()
		{
			// foreach (DestroyLayerInfo info in _destroyInfo.Values)
			// {
			// 	info.timer.Stop();
			// }
			_destroyInfo.Clear();
			running = false;
		}

		private void Awake()
		{
			canvas = GetComponentInParent<Canvas>();
		}

		private void Update()
		{
			if (!running) return;
			foreach (DestroyLayerInfo info in _destroyInfo.Values) info.timer?.Update();
		}

		private void HandleLayer(BackgroundLayer pLayer)
		{
			switch (pLayer.scrollMode)
			{
				case BackgroundLayer.ScrollMode.Repeat:
					GameObject repeatScroller = new GameObject("RepeatScroller");
					repeatScroller.transform.position = pLayer.image.transform.position;
					repeatScroller.transform.SetParent(transform);
					repeatScroller.transform.localScale = Vector3.one;
					repeatScroller.AddComponent<RepeatScroller>().Initialize(pLayer, this);
					break;
				
				case BackgroundLayer.ScrollMode.Destroy:

					DestroyLayerInfo info;

					if (!_destroyInfo.ContainsKey(pLayer))
					{
						PausableElseTimer timer = new();
						GameObject newContainer = new(pLayer.name + " container");
						newContainer.transform.SetParent(transform);
						newContainer.transform.localScale = Vector3.one;
						newContainer.transform.localPosition = Vector3.zero;
						DestroyLayerInfo destroyLayerInfo = new()
						{
							timer = timer,
							container = newContainer,
							offsetHappened = false,
						};
						_destroyInfo.Add(pLayer, destroyLayerInfo);
					}

					info = _destroyInfo[pLayer];


					if (!info.offsetHappened)
					{
						if (pLayer.offsetSeconds != 0)
						{
							info.timer.StartTimer(pLayer.offsetSeconds, () =>
							{
								info.offsetHappened = true;
								HandleLayer(pLayer);
							});
						}
						else
						{
							info.offsetHappened = true;
							HandleLayer(pLayer);
						}
					}
					else
					{
						GameObject go = Instantiate(pLayer.image, info.container.transform);
						LayerScroller scroller = go.AddComponent<LayerScroller>();
						scroller.Initialize(pLayer, this);


						if (pLayer.randYFactor != 0)
						{
							RectTransform rectTransform = go.GetComponent<RectTransform>();
						
							Vector3[] corners = new Vector3[4];
							rectTransform.GetWorldCorners(corners);
							float height = corners[1].y - corners[0].y;
							float midY = corners[1].y - height / 2;
							float newY = Random.Range(midY - height * pLayer.randYFactor, midY + height * pLayer.randYFactor);

							Vector3 position = rectTransform.position;
							position = new Vector3(position.x, newY, position.z);
							rectTransform.position = position;
						}
						
						info.timer.StartTimer(Random.Range(pLayer.minIntervalSeconds, pLayer.maxIntervalSeconds), 
							() => HandleLayer(pLayer));
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			pLayer.image.SetActive(false);
		}
	}

	public class RepeatScroller : MonoBehaviour
	{
		private bool _initialized;
		private BackgroundLayer _layer;
		private BackgroundScroller _scroller;

		private Rect _lRect;
		private Rect _rRect;
		private RectTransform _lTransform;
		private RectTransform _rTransform;

		private GameObject _left;
		private GameObject _right;

		private float _worldWidth;

		private float _startX;
		
		public void Initialize(BackgroundLayer pLayer, BackgroundScroller pScroller)
		{
			_layer = pLayer;
			_scroller = pScroller;
			
			Transform thisTransform = transform;
			Vector3 thisPosition = thisTransform.position;
			
			_left = Instantiate(pLayer.image, thisPosition, Quaternion.identity, thisTransform);
			_lTransform = _left.GetComponent<RectTransform>();
			_lRect = _lTransform.rect;
			_startX = _lTransform.position.x;

			_right = Instantiate(pLayer.image, thisPosition, Quaternion.identity, thisTransform);
			_rTransform = _right.GetComponent<RectTransform>();
			_rRect = _rTransform.rect;
			
			_left.SetActive(true);
			_right.SetActive(true);

			Vector3[] cornersArray = new Vector3[4];
			_lTransform.GetWorldCorners(cornersArray);

			_worldWidth = cornersArray[2].x - cornersArray[1].x;

			
			float distance = cornersArray[2].x - cornersArray[0].x;
			
			_rTransform.Translate(new Vector3(distance,0,0));
	
			_initialized = true;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A)) _rRect.Set(_lRect.position.x + _lRect.width, _lRect.position.y, _rRect.width, _rRect.height);

		}

		private void FixedUpdate()
		{
			if (!_initialized) return;
			if (!_scroller.running) return;
			
			RectTransform lTransform = _left.GetComponent<RectTransform>();
			RectTransform rTransform = _right.GetComponent<RectTransform>();
			
			lTransform.Translate(Vector3.left * _layer.speed);
			rTransform.Translate(Vector3.left * _layer.speed);


			Vector3[] lCorners = new Vector3[4];
			lTransform.GetWorldCorners(lCorners);
			

			Vector3[] rCorners = new Vector3[4];
			rTransform.GetWorldCorners(rCorners);


			if (_lTransform.position.x < _startX - _worldWidth)
			{
				_lTransform.position = _rTransform.position;
				_lTransform.Translate(new Vector3(_worldWidth,0,0));
			}
			
			else if (_rTransform.position.x < _startX - _worldWidth)
			{
				_rTransform.position = _lTransform.position;
				_rTransform.Translate(new Vector3(_worldWidth,0,0));
			}
		}
	}
	
	public class LayerScroller : MonoBehaviour
	{
		private bool _initialized;
		private BackgroundLayer _layer;
		private BackgroundScroller _scroller;
		private PausableElseTimer _timer;

		public void Initialize(BackgroundLayer pLayer, BackgroundScroller pScroller)
		{
			_layer = pLayer;
			_scroller = pScroller;

			_timer = new PausableElseTimer();
			_timer.StartTimer(pLayer.lifeTime, () =>
			{
				Destroy(gameObject);
			});
			
			gameObject.SetActive(true);
			_initialized = true;
		}

		private void Update()
		{
			if (!_initialized) return;
			if (!_scroller.running) return;
			_timer.Update();
		}

		private void FixedUpdate()
		{
			if (!_initialized) return;
			if (!_scroller.running) return;
			transform.position += Vector3.left * _layer.speed;
		}
	}
	
	[Serializable]
	public class BackgroundLayer
	{
		public enum ScrollMode
		{
			Repeat,
			Destroy,
		}
		
		public GameObject image;
		public ScrollMode scrollMode = ScrollMode.Repeat;
		public string name;
		public float speed = 1;
		
		[Header("When not repeating")]
		public float offsetSeconds = 1;
		public float minIntervalSeconds = 0;
		public float maxIntervalSeconds = 0;
		public float randYFactor = 0;
		public float lifeTime = 5;

		private int _z;

		public int z
		{
			set => _z = value;
			get => _z;
		}
	}
}