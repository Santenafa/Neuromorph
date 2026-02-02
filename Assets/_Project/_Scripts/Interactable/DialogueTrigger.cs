using UnityEngine;

namespace Neuromorph
{
    public class DialogueTrigger: Interactable
    {
        [SerializeField] StateMachine stateMachine;
        
        [Header("-------- Ink JSON --------")]
        [SerializeField] TextAsset _inkJson;
        
        [Header("-------- Visual Cue --------")]
        [SerializeField] GameObject _dialogueIcon;

        bool IsTalking => stateMachine.IsCurrentState<DialogueState>();

        void Awake() => _dialogueIcon.SetActive(false);

        public override void Interact()
        { 
            if (IsTalking) return;
            EventBus.OnStartDialogue?.Invoke(_inkJson);
            _dialogueIcon.SetActive(false);
        }

        void OnMouseOver()
        {
            if (!IsTalking) _dialogueIcon.SetActive(true);
        }

        void OnMouseExit() => _dialogueIcon.SetActive(false);
    }
}