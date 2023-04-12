using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main._Scripts.DragonBlower.BlowStates;
using UnityEngine;

namespace Main._Scripts.DragonBlower
{
	public class BlowStateManager : MonoBehaviour
	{
		private BlowStateManager _instance;
		private static List<BlowState> _states = new();

		[CanBeNull]
		private static BlowState FindState(string pName)
		{
			foreach (BlowState state in _states)
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

			_states = GetComponents<BlowState>().ToList();
		}

		private void FixedUpdate()
		{
			if (_currentState != null) _currentState.FixedStateUpdate();
		}

		[CanBeNull] 
		private static BlowState _currentState;
		
		public static void SetState(Type pStateType) => SetState(FindState(pStateType.Name));
		
		public static void SetState(BlowState pState)
		{
			if (pState == null) Debug.LogError("PState is null");
			
			if (_currentState != null) _currentState.Stop();
			
			_currentState = pState;
			
			if (_currentState != null) _currentState.Initiate();
		}
	}
}