using UnityEngine;

namespace User.Wiebe.Scripts.States
{
    public class IdleState : BaseAnimationState
    {
        public IdleState(Animator animator) : base(animator)
        {
        }

        public override void StartAnimation()
        {
            _animator.SetBool("sit", false);
            _animator.SetBool("lie down", false);
            _animator.SetBool("dance", false);
        }

        public override void StopAnimation()
        {
            
        }
    }
}