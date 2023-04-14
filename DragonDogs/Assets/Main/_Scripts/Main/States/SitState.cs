using UnityEngine;
using User.Wiebe.Scripts.States.StateManager;

namespace User.Wiebe.Scripts.States
{
    public class SitState : BaseAnimationState
    {
        public SitState(Animator animator) : base(animator)
        {
            
        }

        public override void StartAnimation()
        {
            _animator.SetBool("sit", true);
        }

        public override void StopAnimation()
        {
            _animator.SetBool("sit", false);
        }
    }
}