using UnityEngine;
using UnityEngine.InputSystem;

namespace Neuromorph
{
    public class InputManager: MonoBehaviour, PlayerInput.IGameplayActions
    {
        [SerializeField] private HostController _playerCharacter;
        private PlayerInput _controls;
        
        private void OnEnable()
        {
            _controls.Gameplay.SetCallbacks(this);
            _controls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            _controls.Gameplay.SetCallbacks(this);
            _controls.Gameplay.Disable();
        }

        private void Awake()
        {
            _controls = new PlayerInput();
        }
        
        public void OnMove(InputAction.CallbackContext context) {
            _playerCharacter.InputDir = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context) {
            if (context.started) _playerCharacter.Jump();
        }

        public void OnInteract(InputAction.CallbackContext context) {
            if (context.started) _playerCharacter.Interact();
        }
        public void OnLook(InputAction.CallbackContext context) {
            if (context.started) _playerCharacter.Interact();
        }
    }
}