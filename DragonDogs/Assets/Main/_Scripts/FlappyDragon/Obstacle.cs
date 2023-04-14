using UnityEngine;

namespace Main._Scripts.FlappyDragon
{
	public class Obstacle : MonoBehaviour
	{
		private bool searching = true;

		private Transform _top;
		private Transform _bot;

		private const int RANGE = 30;
		
		private void Start()
		{
			tag = "Obstacle";
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).tag = "Obstacle";
			}
			
			_top = transform.GetChild(0);
			_bot = transform.GetChild(1);
		}
		
		public void Move(float pSpeed)
		{
			transform.position += Vector3.left * pSpeed * Time.fixedDeltaTime;
		}

		private void FixedUpdate()
		{
			if (!searching) return;

			
			
			#if UNITY_EDITOR
			Debug.DrawRay(_bot.position, Vector3.up * RANGE, Color.red);
			#endif
			
			if (Physics.Raycast(_bot.position, Vector3.up, out RaycastHit hit, RANGE));
			{
				if (hit.transform == null) return;
				if (!hit.transform.CompareTag("Player")) return;
				FlappyDragon.instance.score++;
				searching = false;
			}
		}
	}
}