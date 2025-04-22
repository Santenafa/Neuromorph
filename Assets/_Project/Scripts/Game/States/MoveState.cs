using Neuromorph.Components;
using UnityEngine;

namespace Neuromorph
{
    public class MoveState: BaseGameState
    {
        [SerializeField] Puppet player;
        public override void OnEnter()
        {
            SetObjects(true);
        }
        public override void OnExit()
        {
            SetObjects(false);
        }

        private void SetObjects(bool value)
        {
            player.GetComponent<MovementComponent>().CanMove = value;
            
            foreach (DialogueComponent dialogue in FindObjectsOfType<DialogueComponent>()) {
                dialogue.CanTalk = value;
            }
        }
    }
}