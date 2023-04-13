using UnityEngine;
using User.Wiebe.Scripts.States.StateManager;

namespace User.Wiebe.Scripts.States
{
    public class LieDownState : BaseAnimationState
    {
        public LieDownState(Animator animator) : base(animator)
        {
            
        }

        public override void StartAnimation()
        {
            _animator.SetBool("sit", true);
            _animator.SetBool("lie down", true);
            Debug.Log("Lying down");
        }

        public override void StopAnimation()
        {
            _animator.SetBool("lie down", false);
        }
    }
}