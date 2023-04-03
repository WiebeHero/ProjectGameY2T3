using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

namespace User._Scripts.FlappyDragon
{
    [RequireComponent(typeof(Rigidbody))]
    public class Dragon : MonoBehaviour
    {
        [Header("Keybindings")]
        [SerializeField] private InputAction _flapAction;
        
        [Header("Properties")]
        [SerializeField] private float _flapForce = 1;

        [SerializeField] private Transform _bottomLimit;
        [SerializeField] private Transform _topLimit;
        
        private Rigidbody _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _flapAction.performed += _ => Perform();
        }

        private void Perform()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * _flapForce, ForceMode.Impulse);
        }

        public void ResetPosition()
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = FlappyDragon.instance.startPosition;
        }
        
        private void OnCollisionEnter(Collision pOther)
        {
            switch (pOther.transform.tag)
            {
                case "Obstacle":
                    FlappyStateManager.SetState(typeof(States.FinishState));
                    break;
            }
        }
        
        public void DisableCollision(bool pDisable)
        {
            _rigidbody.detectCollisions = !pDisable;
        }
        
        public void ResetVelocity()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        public void EnableGravity(bool pEnable)
        {
            _rigidbody.useGravity = pEnable;
        }

        public void LockInput(bool pLock)
        {
            if (pLock) _flapAction.Disable();
            else _flapAction.Enable();
        }

        private void FixedUpdate()
        {
            if (transform.position.y < _bottomLimit.position.y)
            {
                FlappyStateManager.SetState(typeof(States.FinishState));
            }
            else if (transform.position.y > _topLimit.position.y)
            {
                FlappyStateManager.SetState(typeof(States.FinishState));
            }
        }
    }
}