using UnityEngine;

namespace Neuromorph.Components
{
    public class MovementComponent : MonoBehaviour
    {
        public bool CanMove { get; set; }
        [SerializeField] private CharacterStatsSO _stats;
        //Walk
        [SerializeField] private CharacterController _controller;
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
            _camera = CameraManager.MainCamera.transform;
        }

        private void Update()
        {
            if (CanMove) Walk();
            ApplyGravity();
        }
        
        private void ApplyGravity()
        {
            if (IsGrounded && _gravityVelocity < 0f) {
                _gravityVelocity = -1f;
            } else {
                _gravityVelocity += Gravity * _stats.GravityMultiplier * Time.deltaTime;
            }
            _controller.Move(_gravityVelocity * Time.deltaTime * Vector3.up);
        }

        private void Walk()
        {
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
    }
}
