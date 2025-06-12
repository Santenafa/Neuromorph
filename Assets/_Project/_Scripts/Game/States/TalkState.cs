using System.Collections;
using System.Collections.Generic;
using Neuromorph.Dialogues;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Neuromorph
{
    public class TalkState : BaseGameState
    {
        public ConState State { get; private set; } = ConState.NotActive;
        
        [SerializeField] private float _textSpeed;
        [SerializeField] private RectTransform _dialoguePanel;
        [SerializeField] private Button _button;
        [SerializeField] private Animator _dialogueAnimator;
        [SerializeField] private GameObject _nameBox;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _dialogueText;
        
        private const string ChooseText = "[Choose your Thought]";
        private string _currentSentence;
        //private DialogueSO _currentDialogueSo;
        //private readonly Queue<Sentence> _sentences = new();
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
            _button.interactable = false;
        }

        public override void OnEnter()
        {
            _button.interactable = true;
            _dialogueAnimator.SetBool(IsOpen, true);
        }
        public override void OnExit()
        {
            _button.interactable = false;
            _dialogueAnimator.SetBool(IsOpen, false);
            SetState(ConState.NotActive);
        }

        private void OnButtonClick()
        {
            switch (State)
            {
                case ConState.AwaitClick: DisplayNextSentence(); break;
                case ConState.Typing: SkipTyping(); break;
                case ConState.Ended: GameManager.ChangeState<MoveState>(); break;
                case ConState.TransitionToAwaitThought:
                    _nameBox.SetActive(false);
                    _dialogueText.text = $"<color={GameColor.PARASITE}>{ChooseText}</color>";
                    ThoughtChooseArea.SetMouth(true);
                    SetState(ConState.AwaitThought);
                    break;
                case ConState.AwaitThought: AcceptThought(); break;
                case ConState.NotActive:
                default: break;
            }
        }

        public void StartDialogue(string dialogueSo)
        {
            //_currentDialogueSo = dialogueSo;
            
            _nameBox.SetActive(true);
            //_nameText.text = _currentDialogueSo.Name;
            //_sentences.Clear();

            //foreach (Sentence sentence in _currentDialogueSo.Sentences)
            //    _sentences.Enqueue(sentence);
            
            //if (_sentences.Count > 0) DisplayNextSentence();
        }

        public void DisplayThought(Thought thought)
        {
            string displayText = thought ? thought.ThoughtData.PromoText : ChooseText;
            _dialogueText.text = $"<color={GameColor.PARASITE}>{displayText}</color>";
        }
        
        private void DisplayNextSentence()
        {
            //_currentSentence = _sentences.Dequeue();
            StopAllCoroutines();
            SetState(ConState.Typing);
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

        private void AcceptThought()
        {
            Thought thought = BrainManager.Instance.ChosenThought;
            if (!thought) return;
            
            //DialogueSO dialogue = thought.ThoughtData.TriggeredDialogue;
            //if (dialogue) StartDialogue(dialogue);
            else GameManager.ChangeState<MoveState>();
            
            BrainManager.Instance.DestroyThought(thought);
            
            ThoughtChooseArea.SetMouth(false);
        }

        private void EndOfSentence()
        {
            //_currentSentence.DialogueEvent.TrySpawnThought();

            /*if (_sentences.Count == 0)
            {
                SetState(_currentDialogueSo.IsAwaitingThought
                    ? ConState.TransitionToAwaitThought
                    : ConState.Ended);
            }
            else SetState(ConState.AwaitClick);*/
        }
        
        private void SetState(ConState newState) => State = newState;
    }
    
    public enum ConState {
        NotActive, AwaitClick, Typing, TransitionToAwaitThought, AwaitThought, Ended
    }
}
