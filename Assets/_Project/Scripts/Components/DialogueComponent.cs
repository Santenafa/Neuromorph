using Neuromorph.Dialogues;
using UnityEngine;
using UnityEngine.UI;

namespace Neuromorph.Components
{
    public class DialogueComponent: MonoBehaviour
    {
        public bool CanTalk { get; set; }

        [SerializeField] private Dialogue _dialogue;
        [SerializeField] private Image _dialogueIcon;
        
        private void StartDialogue()
        {
            if (CanTalk) {
                GameManager.ChangeState<DialogueState>().StartDialogue(_dialogue);
                _dialogueIcon.enabled = false;
            }
        }
        
        private void OnMouseDown()
        {
            StartDialogue();
        }

        private void OnMouseOver()
        {
            if (CanTalk) _dialogueIcon.enabled = true;
        }

        private void OnMouseExit()
        {
            _dialogueIcon.enabled = false;
        }
    }
}