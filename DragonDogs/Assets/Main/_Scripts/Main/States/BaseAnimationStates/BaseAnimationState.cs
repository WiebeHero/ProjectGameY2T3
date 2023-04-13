using UnityEngine;

namespace User.Wiebe.Scripts.States
{
    public abstract class BaseAnimationState : MonoBehaviour, AnimationState
    {
        [SerializeField] protected Animator _animator;

        protected BaseAnimationState(Animator animator)
        {
            _animator = animator;
        }
        
        public abstract void StartAnimation();

        public abstract void StopAnimation();
    }
}