using System.Collections;
using System.Collections.Generic;
using Neuromorph.Utilities;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Neuromorph.Dialogues
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] private float _textSpeed;
        [SerializeField] private Canvas _dialogueCanvas;
        [SerializeField] private Button _button;
        [SerializeField] private Animator _dialogueAnimator;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _dialogueText;
        
        private DialogueState _dialogueState = DialogueState.NotActive;
        private enum DialogueState { NotActive, AwaitClick, Typing, Ended }
        private Dialogue _currentDialogue;
        private string _currentSentence;
        private readonly Queue<string> _sentences = new();
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void OnButtonClick()
        {
            switch (_dialogueState)
            {
                case DialogueState.AwaitClick: DisplayNextSentence(); break;
                case DialogueState.Typing: SkipTyping(); break;
                case DialogueState.Ended: EndDialogue(); break;
                case DialogueState.NotActive:
                default: break;
            }
        }

        public void StartDialogue(Dialogue dialogue)
        {
            _button.onClick.AddListener(OnButtonClick);
            _dialogueAnimator.SetBool(IsOpen, true);
            
            _currentDialogue = dialogue;
            
            _nameText.text = dialogue.Name;
            _sentences.Clear();

            foreach (string sentence in dialogue.Sentences)
                _sentences.Enqueue(sentence);
            
            DisplayNextSentence();
        }

        private void DisplayNextSentence()
        {
            _currentSentence = _sentences.Dequeue();
            StopAllCoroutines();
            _dialogueState = DialogueState.Typing;
            StartCoroutine(TypeSentence());
        }

        private IEnumerator TypeSentence()
        {
            _dialogueText.text = "";
            foreach (char letter in _currentSentence)
            {
                _dialogueText.text += letter;
                yield return new WaitForSeconds(_textSpeed);
            }
            EndOfSentence();
        }
        
        private void SkipTyping()
        {
            StopAllCoroutines();
            _dialogueText.text = _currentSentence;
            EndOfSentence();
        }

        private void EndOfSentence()
        {
            _dialogueState = _sentences.Count == 0 ? DialogueState.Ended : DialogueState.AwaitClick;
        }

        private void EndDialogue()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _dialogueAnimator.SetBool(IsOpen, false);
            
            _dialogueState = DialogueState.NotActive;
            if (_currentDialogue.ThoughtToSpawn)
                BrainManager.Instance.SpawnThought(_currentDialogue.ThoughtToSpawn);
        }
    }
}
