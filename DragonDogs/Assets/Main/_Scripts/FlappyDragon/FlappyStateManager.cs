using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main._Scripts;
using UnityEngine;

namespace User._Scripts.FlappyDragon
{
	public class FlappyStateManager : MonoBehaviour
	{
		private FlappyStateManager _instance;
		private static List<CustomState> _states = new();

		[CanBeNull]
		private static CustomState FindState(string pName)
		{
			foreach (CustomState state in _states)
			{
				if (state.GetType().Name == pName) 
					return state;
			}

			return null;
		}
		
		private void Awake()
		{
			if (_instance == null) _instance = this;
			else
			{
				Debug.LogWarning("Can't have more than one FlappyStateManager in the scene!");
				Destroy(this);
			}

			_states = GetComponents<CustomState>().ToList();
		}

		[CanBeNull] 
		private static CustomState _currentState;
		
		public static void SetState(Type pStateType) => SetState(FindState(pStateType.Name));
		
		public static void SetState(CustomState pState)
		{
			if (_currentState != null) _currentState.Stop();
			
			_currentState = pState;
			
			if (_currentState != null) _currentState.StateStart();
		}

		
		private void Update()
		{
			if (_currentState != null) _currentState.StateUpdate();
		}

		private void FixedUpdate()
		{
			if (_currentState != null) _currentState.FixedStateUpdate();
		}
	}
}