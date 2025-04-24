using UnityEngine;

namespace Neuromorph
{
    public class MoveState: BaseGameState
    {
        public override void OnEnter()
        {
            SetMoveAndTalk(true);
        }
        public override void OnExit()
        {
            SetMoveAndTalk(false);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) GameManager.Player.ClickToMove();
        }

        private static void SetMoveAndTalk(bool value)
        {
            GameManager.Player.SetCanMove(value);
            
            foreach (DialogueComponent dialogue in FindObjectsOfType<DialogueComponent>()) {
                dialogue.CanTalk = value;
            }
        }
    }
}