using UnityEngine;

namespace Main._Scripts.FlappyDragon
{
	public class Obstacle : MonoBehaviour
	{
		private bool searching = true;

		private Transform _top;
		
		private void Start()
		{
			tag = "Obstacle";
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).tag = "Obstacle";
			}
			
			_top = transform.GetChild(0);
		}
		
		public void Move(float pSpeed)
		{
			transform.position += Vector3.left * pSpeed * Time.fixedDeltaTime;
		}

		private void FixedUpdate()
		{
			if (!searching) return;

			#if UNITY_EDITOR
			Debug.DrawRay(_top.position, Vector3.down * 10, Color.red);
			#endif

			if (Physics.Raycast(_top.position, Vector3.down, out RaycastHit hit, 5))
			{
				if (!hit.transform.CompareTag("Player")) return;
				
				FlappyDragon.instance.score++;
				searching = false;
			}
		}
	}
}