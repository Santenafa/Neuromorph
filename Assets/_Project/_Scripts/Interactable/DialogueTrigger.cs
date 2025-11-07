using UnityEngine;

namespace Neuromorph
{
    public class DialogueTrigger: Interactable
    {
        [SerializeField] GameManager _gameManager;
        
        [Header("-------- Ink JSON --------")]
        [SerializeField] TextAsset _inkJson;
        
        [Header("-------- Visual Cue --------")]
        [SerializeField] GameObject _dialogueIcon;

        bool IsTalking => _gameManager.IsCurrentState<DialogueState>();

        void Awake() => _dialogueIcon.SetActive(false);

        public override void Interact()
        { 
            if (IsTalking) return;
            _gameManager.GetState<DialogueState>().EnterDialogue(_inkJson);
            _dialogueIcon.SetActive(false);
        }

        void OnMouseOver()
        {
            if (!IsTalking) _dialogueIcon.SetActive(true);
        }

        void OnMouseExit() => _dialogueIcon.SetActive(false);
    }
}