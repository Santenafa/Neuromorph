using UnityEngine.UI;
using Ink.Runtime;
using Neuromorph.Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Neuromorph
{
    public class DialogueState: BaseGameState
    {
        [Header("-------- Properties --------")]
        [SerializeField] private float _typeSpeed = 15f;
        
        [Header("-------- Animator --------")]
        [SerializeField] private Animator _dialogueAnimator;
        
        [Header("-------- Line Display --------")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private GameObject _canClickIcon;
        
        [Header("-------- Choice --------")]
        [SerializeField] private Button _choiceButton;
        [SerializeField] private TMP_Text _choiceText;
        
        [Header("-------- Name --------")]
        [SerializeField] private GameObject _nameBox;
        [SerializeField] private TMP_Text _nameText;
        
        // -------- Constants --------
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        
        // -------- Private --------
        private Story _currentStory;
        private TMP_Typewriter _typeWriter;
        private bool IsChoosing => _currentStory.currentChoices.Count > 0;
        private int? _chooseIndex;

        //========== MonoBehavior ==========
        private void Awake() =>
            BrainManager.OnChosenThought += SetChoice;
        private void OnDestroy() =>
            BrainManager.OnChosenThought -= SetChoice;
        
        private void Start()
        {
            _continueButton.onClick.AddListener(OnContinueButton);
            _choiceButton.onClick.AddListener(OnChoiceButton);
            _continueButton.interactable = false;
            _choiceButton.gameObject.SetActive(false);
            _canClickIcon.SetActive(false);
            _typeWriter = new TMP_Typewriter(_dialogueText);
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                OnContinueButton();
        }
        
        //========== Base State ==========
        public override void OnEnter()
        {
            _continueButton.interactable = true;
            _dialogueAnimator.SetBool(IsOpen, true);
        }
        public override void OnExit()
        {
            _continueButton.interactable = false;
            _dialogueAnimator.SetBool(IsOpen, false);
        }

        //========== Enter / Exit ==========
        public void EnterDialogue(TextAsset inkJson)
        {
            _currentStory = new Story(inkJson.text);
            InkExternalFunctions.Bind(_currentStory, this);
            UpdateDialogueBox();
        }
        
        private void ExitDialogue()
        {
            InkExternalFunctions.Unbind(_currentStory);
            GameManager.ChangeState<MoveState>();
        }
        
        //========== Buttons ==========
        private void OnContinueButton()
        {
            if (_typeWriter.IsTyping)
                _typeWriter.Skip();
            else if (_currentStory.canContinue)
                UpdateDialogueBox();
            else ExitDialogue();
        }
        /// <summary> Make Choice </summary>
        private void OnChoiceButton()
        {
            if (_chooseIndex is null) return;
            
            _currentStory.ChooseChoiceIndex((int)_chooseIndex);
            BrainManager.Instance.DestroyThought(BrainManager.Instance.ChosenThought);
            
            UpdateChoiceMode();
            _choiceButton.gameObject.SetActive(false);
            OnContinueButton();
        }
        //========== Update Dialogue Box ==========
        private void UpdateDialogueBox()
        {
            _canClickIcon.SetActive(false);
            _nameBox.SetActive(false);
            string rawLine = _currentStory.Continue();
            string newLine = InkHandlers.HandleNames(rawLine, _nameBox, _nameText);

            if (newLine.Equals("") && !_currentStory.canContinue)
            {
                ExitDialogue();
                return;
            }

            _typeWriter.Play(newLine, _typeSpeed, () => {
                if (!IsChoosing) _canClickIcon.SetActive(true);
            });
            InkHandlers.HandleTags(_currentStory.currentTags);
            
            UpdateChoiceMode();
        }
        
        //========== Choosing ==========
        private void UpdateChoiceMode()
        {
            //TODO: Open/Close Choice Animation
            _continueButton.interactable = !IsChoosing;
            _canClickIcon.SetActive(false);
            ThoughtChooseArea.SetMouth(IsChoosing);
        }
        
        private void SetChoice(Thought thought)
        {
            if (!(IsChoosing && thought))
            {
                Hide();
                return;
            }
            
            Choice rightChoice = null;
            foreach (Choice choice in _currentStory.currentChoices)
            {
                if (InkHandlers.HandleChoices(choice.text, out string keyWord, out string _)
                    && thought.Name == keyWord)
                {
                    rightChoice = choice;
                }
            }

            if (rightChoice != null)
            {
                InkHandlers.HandleChoices(rightChoice.text, out string keyWord, out string promoLine);

                _chooseIndex = rightChoice.index;
                _choiceButton.gameObject.SetActive(true);
                _choiceText.text = $"<color={GameColor.PARASITE}>{promoLine}</color>";
                thought.SetState(ThoughtState.Chosen);
            }
            else Hide();

            return;

            void Hide()
            {
                _chooseIndex = null;
                _choiceButton.gameObject.SetActive(false);
            }
        }
    }
}