using DG.Tweening;
using UnityEngine.UI;
using Ink.Runtime;
using Neuromorph.Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Neuromorph
{
public class DialogueState: GameState
{
    [Header("-------- Properties --------")]
    [SerializeField] float _typeSpeed = 10f;
    [SerializeField] float _dialogueOpenDuration = 0.5f;
    [SerializeField] float _chooseBoxOpenDuration = 0.5f;
    
    [Header("-------- Animator --------")]
    [SerializeField] RectTransform _dialogueRect;
    [SerializeField] LayoutElement _choiceBox;
    float _openDialoguePosY;
    float _closeDialoguePosY;
    
    [Header("-------- Line Display --------")]
    [SerializeField] Button _continueButton;
    [SerializeField] TMP_Text _dialogueText;
    [SerializeField] GameObject _canClickIcon;
    
    [Header("-------- Choice --------")]
    [SerializeField] Button _choiceButton;
    [SerializeField] TMP_Text _choiceText;
    [SerializeField] ThoughtChooseArea _thoughtChooseArea;
    
    [Header("-------- Name --------")]
    [SerializeField] GameObject _nameBox;
    [SerializeField] TMP_Text _nameText;
    
    // -------- Private --------
    Story _currentStory;
    Typewriter _typeWriter;
    bool CanContinue => _currentStory && _currentStory.canContinue;
    bool IsChoosing => _currentStory && _currentStory.currentChoices.Count > 0;
    int? _chooseIndex;

    //========== MonoBehavior ==========
    void OnEnable()
    {
        EventBus.OnStartDialogue += EnterDialogue;
        EventBus.OnChosenThought += SetChoice;
    }
    void OnDisable()
    {
        EventBus.OnStartDialogue -= EnterDialogue;
        EventBus.OnChosenThought -= SetChoice;
    }

    void Start()
    {
        _continueButton.onClick.AddListener(OnContinueButton);
        _choiceButton.onClick.AddListener(OnChoiceButton);
        
        _continueButton.interactable = false;
        _choiceButton.gameObject.SetActive(false);
        _canClickIcon.SetActive(false);
        _choiceBox.flexibleWidth = 0;
        
        _typeWriter = new Typewriter(_dialogueText);
        
        _openDialoguePosY = _dialogueRect.anchoredPosition.y;
        _closeDialoguePosY = _dialogueRect.anchoredPosition.y - _dialogueRect.rect.height * 2;
        _dialogueRect.anchoredPosition
            = new Vector2(_dialogueRect.anchoredPosition.x, _closeDialoguePosY);
    }
    
    //========== Base State ==========
    public override void OnEnter()
    {
        InkExternalFunctions.Bind(_currentStory);
        UpdateDialogueBox();
        _continueButton.interactable = true;
        _dialogueRect.DOAnchorPosY(_openDialoguePosY, _dialogueOpenDuration);
    }
    public override void OnExit()
    {
        InkExternalFunctions.Unbind(_currentStory);
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
    void EnterDialogue(TextAsset inkJson)
    {
        _currentStory = new Story(inkJson.text);
        StateMachine.ChangeState<DialogueState>(); //On Enter logic
    }

    void ExitDialogue() => StateMachine.ChangeState<MoveState>(); //On Exit logic
    
    //========== Buttons ==========
    void OnContinueButton()
    {
        if (IsChoosing) return;

        if (_typeWriter.IsTyping) _typeWriter.Skip();
        else if (CanContinue) UpdateDialogueBox();
        else ExitDialogue();
    }
    /// <summary> Make Choice </summary>
    void OnChoiceButton()
    {
        if (_chooseIndex is null) return;
        
        _currentStory.ChooseChoiceIndex((int)_chooseIndex);
        BrainManager.Instance.DestroyThought(BrainManager.Instance.ChosenThought);
        
        UpdateChoiceMode();
        _choiceButton.gameObject.SetActive(false);
        OnContinueButton();
    }
    //========== Update Dialogue Box ==========
    void UpdateDialogueBox()
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
    void EnterChoiceMode()
    {
        _choiceBox.DOFlexibleSize(new Vector2(0.5f, 0f) , _chooseBoxOpenDuration);
        UpdateChoiceMode();
    }

    void ExitChoiceMode()
    {
        _choiceBox.DOFlexibleSize(Vector2.zero , _chooseBoxOpenDuration)
            .OnComplete(UpdateChoiceMode);
    }

    void UpdateChoiceMode()
    {
        _continueButton.interactable = !IsChoosing;
        if (IsChoosing) _canClickIcon.SetActive(false);
        _thoughtChooseArea.SetMouth(IsChoosing);
    }

    void SetChoice(Thought thought)
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
            _choiceText.text = $"<color=#239063>{promoLine}</color>";
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