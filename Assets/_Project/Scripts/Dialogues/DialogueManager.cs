using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] private RectTransform _dialoguePanel;
        [SerializeField] private Button _button;
        [SerializeField] private Animator _dialogueAnimator;
        [SerializeField] private GameObject _nameBox;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _dialogueText;
        
        private const string ChooseText = "[Choose your Thought]";
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
                    _nameBox.SetActive(false);
                    _dialogueText.text = $"<color={GameColor.PARASITE}>{ChooseText}</color>";
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
            
            _nameBox.SetActive(true);
            _nameText.text = _currentDialogue.Name;
            _sentences.Clear();

            foreach (Sentence sentence in _currentDialogue.Sentences)
                _sentences.Enqueue(sentence);
            
            if (_sentences.Count > 0) DisplayNextSentence();
        }

        public static void DisplayThought(Thought thought)
        {
            string displayText = thought ? thought.ThoughtData.PromoText : ChooseText;
            Instance._dialogueText.text = $"<color={GameColor.PARASITE}>{displayText}</color>";
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
                SetState(_currentDialogue.IsAwaitingThought
                    ? DialogueState.TransitionToAwaitThought
                    : DialogueState.Ended);
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
    
    public enum DialogueState {
        NotActive, AwaitClick, Typing, TransitionToAwaitThought, AwaitThought, Ended
    }
}
