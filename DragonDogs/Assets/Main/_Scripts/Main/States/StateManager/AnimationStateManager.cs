using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace User.Wiebe.Scripts.States.StateManager
{
    public class AnimationStateManager : MonoBehaviour
    { 
        private AnimationStateManager _instance;
        private static List<AnimationState> _states = new();

        [CanBeNull]
        private static AnimationState FindState(string pName)
        {
            foreach (AnimationState state in _states)
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
                Debug.LogWarning("Can't have more than one AnimationStateManager in the scene!");
                Destroy(this);
            }
            _states = GetComponents<AnimationState>().ToList();
        }

        [CanBeNull] 
        private static AnimationState _currentState;
		
        public static void SetState(Type pStateType) => SetState(FindState(pStateType.Name));
		
        public static void SetState(AnimationState pState)
        {
            if (_currentState != null) _currentState.StopAnimation();
			
            _currentState = pState;
			
            if (_currentState != null) _currentState.StartAnimation();
        }
    }
}