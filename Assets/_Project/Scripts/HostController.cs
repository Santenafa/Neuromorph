using Neuromorph.Interfaces;
using UnityEngine;

namespace Neuromorph
{
    [RequireComponent(typeof(CharacterController))]
    public class HostController : MonoBehaviour
    {
        [SerializeField] private CharacterStatsSO _stats;
        //Walk
        private CharacterController _controller;
        public Vector2 InputDir { get; set; }
        //Rotation
        private float _currentVelocity; 
        private Transform _camera;
        //Gravity
        private bool _isGrounded => _controller.isGrounded; 
        private const float Gravity = -9.81f;
        private float _gravityVelocity;
        
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _camera = Camera.main.transform;
        }

        private void Update()
        {
            Walk();
            ApplyGravity();
        }
        
        private void ApplyGravity() {
            if (_isGrounded && _gravityVelocity < 0f) {
                _gravityVelocity = -1f;
            } else {
                _gravityVelocity += Gravity * _stats.GravityMultiplier * Time.deltaTime;
            }
            _controller.Move(_gravityVelocity * Time.deltaTime * Vector3.up);
        }

        private void Walk() {
            if (InputDir.sqrMagnitude == 0) return;
            
            float targetAngle
                = Mathf.Atan2(InputDir.x, InputDir.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, targetAngle, ref _currentVelocity, _stats.RotationSmoothTime
            );
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveAngle = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(_stats.WalkSpeed * Time.deltaTime * moveAngle);
        }
        
        public void Jump() {
            if (!_isGrounded) return;
            
            _gravityVelocity += _stats.JumpPower;
        }

        public void Interact() {
            if (! Physics.Raycast(
                    transform.position, Vector3.forward,
                    out RaycastHit hitInfo, _stats.InteractRange)
                ) return;
            
            hitInfo.collider.GetComponent<IInteractable>()?.Interact();
        }
    }
}
