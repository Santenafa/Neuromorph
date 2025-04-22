using Neuromorph.Dialogues;
using UnityEngine;
using UnityEngine.UI;

namespace Neuromorph.Components
{
    public class DialogueComponent: BaseComponent
    {
        [SerializeField] private Dialogue _dialogue;
        [SerializeField] private Image _dialogueIcon;
        
        public void StartDialogue()
        {
            DialogueManager.Instance.StartDialogue(_dialogue);
        }
        
        private void OnMouseDown()
        {
            StartDialogue();
        }

        private void OnMouseOver()
        {
            _dialogueIcon.enabled = true;
        }

        private void OnMouseExit()
        {
            _dialogueIcon.enabled = false;
        }
    }
}