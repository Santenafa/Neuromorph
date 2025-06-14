using UnityEngine;

namespace Neuromorph
{
    public class DialogueTrigger: Interactable
    {
        [Header("-------- Ink JSON --------")]
        [SerializeField] private TextAsset _inkJson;
        
        [Header("-------- Visual Cue --------")]
        [SerializeField] private GameObject _dialogueIcon;

        private static bool IsTalking => GameManager.IsCurrentState<DialogueState>();
        private void Awake()
        {
            _dialogueIcon.SetActive(false);
        }

        public override void Interact()
        { 
            if (IsTalking) return;
            GameManager.GetState<DialogueState>()
                .EnterDialogue(_inkJson);
            _dialogueIcon.SetActive(false);
        }

        private void OnMouseOver()
        {
            if (!IsTalking) _dialogueIcon.SetActive(true);
        }

        private void OnMouseExit() => _dialogueIcon.SetActive(false);
    }
}