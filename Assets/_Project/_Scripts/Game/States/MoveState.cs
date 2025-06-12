using UnityEngine.InputSystem;

namespace Neuromorph
{
    public class MoveState: BaseGameState
    {
        public override void OnEnter()
        {
            GameManager.Player.SetCanMove(true);
        }
        public override void OnExit()
        {
            GameManager.Player.SetCanMove(false);
        }

        public override void OnUpdate()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
                GameManager.Player.ClickToMove();
        }
    }
}