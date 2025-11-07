using UnityEngine;
using UnityEngine.SceneManagement;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Neuromorph
{
    public class InputManager: MonoBehaviour, PlayerInput.IGameplayActions, PlayerInput.IUIActions
    {
        [SerializeField] Puppet _puppet;
        PlayerInput _input;

        void OnEnable()
        {
            _input.Gameplay.SetCallbacks(this);
            _input.Gameplay.Enable();
        }

        void OnDisable()
        {
            _input.Gameplay.SetCallbacks(this);
            _input.Gameplay.Disable();
        }

        protected void Awake()
        {
            _input = new PlayerInput();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (Input.GetKeyDown(KeyCode.C)) Application.Quit();

            /*if (Input.GetKeyDown(KeyCode.Q)
                && _puppet.TryGetComponent(out PossessionComponent possession)
                && possession.TryPossess(out Puppet puppet))
            {
                PossessionComponent.UnPossess(_puppet); //Memory Wipe
                _puppet = puppet;
                print("new puppet: " + puppet.name);
            }*/
        }

        public void OnMove(CallbackContext context)
        {
            //_puppet.Movement.InputDir = context.ReadValue<Vector2>();
        }

        public void OnClickMove(CallbackContext context)
        {
            //if (context.started && GameManager.IsCurrentState<MoveState>())
            //  GameManager.Player.ClickToMove();
        }

        public void OnOpenMenu(CallbackContext context)
        {
            //if (context.started) Application.Quit();
        }
    }
}