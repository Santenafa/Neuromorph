using Neuromorph.Dialogues;
using UnityEngine;
using UnityEngine.UI;

namespace Neuromorph
{
    public class DialogueComponent: Interactable
    {
        public bool CanTalk { get; set; }

        [SerializeField] private DialogueSO _dialogue;
        [SerializeField] private Image _dialogueIcon;
        
        private void StartDialogue()
        {
            if (!CanTalk) return;
            
            GameManager.ChangeState<TalkState>().StartDialogue(_dialogue);
            _dialogueIcon.enabled = false;
        }
        
        public override void Interact()
        {
            StartDialogue();
        }
        
        private void OnMouseDown()
        {
            //StartDialogue();
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