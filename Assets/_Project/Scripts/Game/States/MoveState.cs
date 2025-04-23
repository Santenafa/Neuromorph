using Neuromorph.Components;

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

        private static void SetMoveAndTalk(bool value)
        {
            GameManager.Player.GetComponent<MovementComponent>().CanMove = value;
            
            foreach (DialogueComponent dialogue in FindObjectsOfType<DialogueComponent>()) {
                dialogue.CanTalk = value;
            }
        }
    }
}