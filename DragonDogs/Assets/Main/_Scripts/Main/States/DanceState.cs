using UnityEngine;

namespace User.Wiebe.Scripts.States
{
    public class DanceState : BaseAnimationState
    {
        public DanceState(Animator animator) : base(animator)
        {
        }

        public override void StartAnimation()
        {
            _animator.SetBool("dance", true);
            _animator.SetBool("lie down", false);
            _animator.SetBool("sit", false);
        }

        public override void StopAnimation()
        {
            _animator.SetBool("dance", false);
        }
    }
}