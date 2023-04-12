using Main._Scripts.DragonBlower.BlowStates;
using UnityEngine;

namespace Main._Scripts.DragonBlower
{
	[RequireComponent(typeof(MicToMovement))]
	public class DragonBlower : MonoBehaviour
	{
		[SerializeField] private int _gameDuration = 5;
		public int gameDuration => _gameDuration;
		
		private MicToMovement _micToMovement;

		public static DragonBlower instance;
		
		public Vector3 startPosition { get; private set; }

		public int score
		{
			get
			{
				float distance = transform.position.y - startPosition.y;
				return (int) distance;
			}
		}

		private void Awake()
		{
			if (instance == null) instance = this;
			else Destroy(gameObject);
			
			_micToMovement = GetComponent<MicToMovement>();
		}

		private void Start()
		{
			startPosition = transform.position;
			BlowStateManager.SetState(typeof(BlowStartState));
		}

		public void Enable(bool pEnable)
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			if (rb == null)	
			{
				Debug.LogWarning("Dragon has no rigidbody!");
				return;
			}
			
			rb.velocity = Vector3.zero;
			rb.useGravity = pEnable;
			_micToMovement.Enable(pEnable);
		}
		
		public void ResetPosition()
		{
			transform.position = startPosition;
		}
	}
}