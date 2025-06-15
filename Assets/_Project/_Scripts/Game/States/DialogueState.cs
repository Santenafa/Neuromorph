using DG.Tweening;
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
        [FormerlySerializedAs("_typeDuration")]
        [Header("-------- Properties --------")]
        [SerializeField] private float _typeSpeed = 10f;
        [SerializeField] private float _dialogueOpenDuration = 0.5f;
        [SerializeField] private float _chooseBoxOpenDuration = 0.5f;
        
        [Header("-------- Animator --------")]
        [SerializeField] private RectTransform _dialogueRect;
        [SerializeField] private LayoutElement _choiceBox;
        private float _openDialoguePosY;
        private float _closeDialoguePosY;
        
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
        
        // -------- Private --------
        private Story _currentStory;
        private TMP_Typewriter _typeWriter;
        private bool CanContinue => _currentStory && _currentStory.canContinue;
        private bool IsChoosing => _currentStory && _currentStory.currentChoices.Count > 0;
        private int? _chooseIndex;

        //========== MonoBehavior ==========
        private void OnEnable() =>
            BrainManager.OnChosenThought += SetChoice;
        private void OnDisable() =>
            BrainManager.OnChosenThought -= SetChoice;
        
        private void Start()
        {
            _continueButton.onClick.AddListener(OnContinueButton);
            _choiceButton.onClick.AddListener(OnChoiceButton);
            
            _continueButton.interactable = false;
            _choiceButton.gameObject.SetActive(false);
            _canClickIcon.SetActive(false);
            _choiceBox.flexibleWidth = 0;
            
            _typeWriter = new TMP_Typewriter(_dialogueText);
            
            _openDialoguePosY = _dialogueRect.anchoredPosition.y;
            _closeDialoguePosY = _dialogueRect.anchoredPosition.y - _dialogueRect.rect.height * 2;
            _dialogueRect.anchoredPosition
                = new Vector2(_dialogueRect.anchoredPosition.x, _closeDialoguePosY);
        }
        
        //========== Base State ==========
        public override void OnEnter()
        {
            _continueButton.interactable = true;
            _dialogueRect.DOAnchorPosY(_openDialoguePosY, _dialogueOpenDuration);
        }
        public override void OnExit()
        {
            _continueButton.interactable = false;
            _dialogueRect.DOAnchorPosY(_closeDialoguePosY, _dialogueOpenDuration);
        }
        public override void OnUpdate()
        {
            if (!Keyboard.current.spaceKey.wasPressedThisFrame) return;
            
            OnContinueButton();
            OnChoiceButton();
        }

        //========== Enter / Exit ==========
        public void EnterDialogue(TextAsset inkJson)
        {
            _currentStory = new Story(inkJson.text);
            InkExternalFunctions.Bind(_currentStory, this);
            UpdateDialogueBox();
            GameManager.ChangeState<DialogueState>(); //On Enter logic
        }
        
        private void ExitDialogue()
        {
            InkExternalFunctions.Unbind(_currentStory);
            GameManager.ChangeState<MoveState>(); //On Exit logic
        }
        
        //========== Buttons ==========
        private void OnContinueButton()
        {
            if (IsChoosing) return;

            if (_typeWriter.IsTyping) _typeWriter.Skip();
            else if (CanContinue) UpdateDialogueBox();
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

            if (newLine.Equals("") && !CanContinue)
            {
                ExitDialogue();
                return;
            }
            
            _typeWriter.Play(newLine, _typeSpeed, () => {
                _canClickIcon.SetActive(!IsChoosing);
            });
            InkHandlers.HandleTags(_currentStory.currentTags);
            
            if (IsChoosing) EnterChoiceMode();
            else ExitChoiceMode();
        }
        
        //========== Choosing ==========
        private void EnterChoiceMode()
        {
            _choiceBox.DOFlexibleSize(new Vector2(0.5f, 0f) , _chooseBoxOpenDuration);
            UpdateChoiceMode();
        }
        private void ExitChoiceMode()
        {
            _choiceBox.DOFlexibleSize(Vector2.zero , _chooseBoxOpenDuration)
                .OnComplete(() =>
                {
                    UpdateChoiceMode();
                });
        }
        private void UpdateChoiceMode()
        {
            _continueButton.interactable = !IsChoosing;
            if (IsChoosing) _canClickIcon.SetActive(false);
            ThoughtChooseArea.SetMouth(IsChoosing);
        }
        
        private void SetChoice(Thought thought)
        {
            if (!(IsChoosing && thought))
            {
                HideChoiceButton();
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
                InkHandlers.HandleChoices(rightChoice.text, out string _, out string promoLine);

                _chooseIndex = rightChoice.index;
                _choiceButton.gameObject.SetActive(true);
                _choiceText.text = $"<color={GameColor.PARASITE}>{promoLine}</color>";
                thought.SetState(ThoughtState.Chosen);
            }
            else HideChoiceButton();

            return;
            void HideChoiceButton()
            {
                _chooseIndex = null;
                _choiceButton.gameObject.SetActive(false);
            }
        }
    }
}