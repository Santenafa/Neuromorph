using Neuromorph.Interfaces;
using UnityEngine;

namespace Neuromorph.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementComponent : BaseComponent
    {
        [SerializeField] private CharacterStatsSO _stats;
        //Walk
        private CharacterController _controller;
        public Vector2 InputDir { get; set; }
        //Rotation
        private float _currentVelocity; 
        private Transform _camera;
        //Gravity
        private bool IsGrounded => _controller.isGrounded; 
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
            if (IsGrounded && _gravityVelocity < 0f) {
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
            if (!IsGrounded) return;
            
            _gravityVelocity += _stats.JumpPower;
        }

        public void Interact() {
            if (!Physics.Raycast( _controller.bounds.center, transform.forward,
                out RaycastHit hitInfo, _stats.InteractRange)) return;
            
            hitInfo.collider.GetComponent<DialogueComponent>()?.StartDialogue();
        }
    }
}
