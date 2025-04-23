using Neuromorph.Utilities;
using Neuromorph.Components;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Neuromorph
{
    public class InputManager: Singleton<InputManager>, PlayerInput.IGameplayActions
    {
        [SerializeField] private Puppet _puppet;
        private PlayerInput _input;
        
        private void OnEnable()
        {
            _input.Gameplay.SetCallbacks(this);
            _input.Gameplay.Enable();
        }

        private void OnDisable()
        {
            _input.Gameplay.SetCallbacks(this);
            _input.Gameplay.Disable();
        }

        protected override void Awake()
        {
            base.Awake();
            _input = new PlayerInput();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)
                && _puppet.TryGetComponent(out PossessionComponent possession)
                && possession.TryPossess(out Puppet puppet))
            {
                PossessionComponent.UnPossess(_puppet); //Memory Wipe
                _puppet = puppet;
                print("new puppet: " + puppet.name);
            }
        }

        public void OnMove(CallbackContext context)
        {
            //_puppet.Movement.InputDir = context.ReadValue<Vector2>();
        }

        public void OnClickMove(CallbackContext context)
        {
            if (context.started) GameManager.Player.ClickToMove();
        }
    }
}