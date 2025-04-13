using System;
using System.Collections;
using System.Collections.Generic;
using Neuromorph.Dialogues.Events;
using Neuromorph.Utilities;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Neuromorph.Dialogues
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        public DialogueState State { get; private set; } = DialogueState.NotActive;
        [SerializeField] private float _textSpeed;
        [SerializeField] private Canvas _dialogueCanvas;
        [SerializeField] private Button _button;
        [SerializeField] private Animator _dialogueAnimator;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _dialogueText;
        private const string ChooseText = "[Choose your Thought]";


        public enum DialogueState
        {
            NotActive, AwaitClick, Typing, TransitionToAwaitThought, AwaitThought, Ended
        }
        private Sentence _currentSentence;
        private Dialogue _currentDialogue;
        private readonly Queue<Sentence> _sentences = new();
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
            _button.interactable = false;
        }

        private void OnButtonClick()
        {
            switch (State)
            {
                case DialogueState.AwaitClick: DisplayNextSentence(); break;
                case DialogueState.Typing: SkipTyping(); break;
                case DialogueState.Ended: EndDialogue(); break;
                case DialogueState.TransitionToAwaitThought: 
                    _nameText.text = "";
                    _dialogueText.text = $"<color={GameColor.BRAIN}>{ChooseText}</color>";
                    SetState(DialogueState.AwaitThought);
                    break;
                case DialogueState.AwaitThought: AcceptThought(); break;
                case DialogueState.NotActive:
                default: break;
            }
        }

        public void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            
            _button.interactable = true;
            _dialogueAnimator.SetBool(IsOpen, true);
            
            _nameText.text = _currentDialogue.Name;
            _sentences.Clear();

            foreach (Sentence sentence in _currentDialogue.Sentences)
                _sentences.Enqueue(sentence);
            
            if (_sentences.Count > 0) DisplayNextSentence();
        }

        public void DisplayThought(Thought thought)
        {
            string displayText = thought ? thought.ThoughtData.PromoText : ChooseText;
            _dialogueText.text = $"<color={GameColor.BRAIN}>{displayText}</color>";
        }
        
        private void DisplayNextSentence()
        {
            _currentSentence = _sentences.Dequeue();
            StopAllCoroutines();
            SetState(DialogueState.Typing);
            StartCoroutine(TypeSentence());
        }

        private IEnumerator TypeSentence()
        {
            _dialogueText.text = "";
            foreach (char letter in _currentSentence.Text)
            {
                _dialogueText.text += letter;
                yield return new WaitForSeconds(_textSpeed);
            }
            EndOfSentence();
        }
        
        private void SkipTyping()
        {
            StopAllCoroutines();
            _dialogueText.text = _currentSentence.Text;
            EndOfSentence();
        }

        private void AcceptThought()
        {
            Thought thought = BrainManager.Instance.ChosenThought;
            if (thought) {
                StartDialogue(thought.ThoughtData.TriggeredDialogue);
                BrainManager.Instance.DestroyThought(thought);
            }
        }

        private void EndOfSentence()
        {
            _currentSentence.DialogueEvent.TrySpawnThought();

            if (_sentences.Count == 0)
            {
                if (_currentDialogue.IsAwaitingThought) {
                    SetState(DialogueState.TransitionToAwaitThought);
                }
                else SetState(DialogueState.Ended);
            }
            else SetState(DialogueState.AwaitClick);
        }

        private void EndDialogue()
        {
            _button.interactable = false;
            _dialogueAnimator.SetBool(IsOpen, false);

            SetState(DialogueState.NotActive);
        }
        
        private void SetState(DialogueState newState) => State = newState;
    }
}
