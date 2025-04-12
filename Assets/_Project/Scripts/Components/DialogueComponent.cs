using Neuromorph.Dialogues;
using UnityEngine;

namespace Neuromorph.Components
{
    public class DialogueComponent: BaseComponent
    {
        [SerializeField] private Dialogue _dialogue;
        
        public void StartDialogue()
        {
            DialogueManager.Instance.StartDialogue(_dialogue);
        }
    }
}