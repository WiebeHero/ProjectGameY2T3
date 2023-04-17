using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

namespace Main._Scripts.FlappyDragon
{
    [RequireComponent(typeof(Rigidbody))]
    public class Dragon : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        [Header("Keybindings")]
        [SerializeField] private InputAction _flapAction;
        
        [Header("Properties")]
        [SerializeField] private float _flapForce = 1;

        [SerializeField] private Transform _bottomLimit;
        [SerializeField] private Transform _topLimit;


        [SerializeField] private float _upRotation = 30;
        [SerializeField] private float _downRotation = 30;

        [SerializeField] private AudioSource flap;
        
        private Rigidbody _rigidbody;
        private static readonly int _Flap = Animator.StringToHash("Flap");

        private bool started, finished;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _flapAction.performed += _ => Perform();
        }

        private void Perform()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * _flapForce, ForceMode.Impulse);
            flap.Play();
            
            _animator.SetTrigger(_Flap);
        }

        public void ResetPosition()
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = FlappyDragon.instance.startPosition;
            started = false;
        }
        
        private void OnCollisionEnter(Collision pOther)
        {
            switch (pOther.transform.tag)
            {
                case "Obstacle":
                    if (!started)
                    {
                        FlappyStateManager.SetState(typeof(States.FinishState));
                        started = true;
                    }
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
            if ((transform.position.y < _bottomLimit.position.y || transform.position.y > _topLimit.position.y) && !started)
            {
                FlappyStateManager.SetState(typeof(States.FinishState));
                started = true;
            }
            
            if (_rigidbody.velocity.y > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _upRotation), Time.deltaTime * 10);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -_downRotation), Time.deltaTime * 10);
            }
        }
    }
}