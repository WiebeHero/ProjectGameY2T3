using UnityEngine;

namespace User.Wiebe.Scripts.States
{
    public class RollOverState : BaseAnimationState
    {
        public RollOverState(Animator animator) : base(animator)
        {
        }

        public override void StartAnimation()
        {
            _animator.SetBool("sit", true);
            _animator.SetBool("lie down", true);
            _animator.SetTrigger("roll");
            Debug.Log("Rolling Over");
        }

        public override void StopAnimation()
        {
            //TO DO
        }
    }
}