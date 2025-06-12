using UnityEngine.UI;
using Ink.Runtime;
using Neuromorph.Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Neuromorph
{
    public class DialogueState: BaseGameState
    {
        public bool IsAwaitThought { get; private set; }
        
        [Header("-------- Properties --------")]
        //[SerializeField] private TextMeshProUGUI _dialogueTextDisplay;
        [SerializeField] private float _textSpeed;
        
        [Header("-------- Components --------")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Animator _dialogueAnimator;
        
        [Header("-------- Text Box --------")]
        [SerializeField] private TMP_Text _dialogueText;
        
        [Header("-------- Name --------")]
        [SerializeField] private GameObject _nameBox;
        [SerializeField] private TMP_Text _nameText;
        
        // -------- Constants --------
        private const string ChooseText = "[Choose your Thought]";
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        
        private Story _currentStory;

        private void Start()
        {
            _continueButton.onClick.AddListener(OnButtonClick);
            _continueButton.interactable = false;
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                OnButtonClick();
        }
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

        public void EnterDialogue(TextAsset inkJson)
        {
            _currentStory = new Story(inkJson.text);
            InkExternalFunctions.Bind(_currentStory);
            ContinueStory();
        }
        
        private void ExitDialogue()
        {
            InkExternalFunctions.Unbind(_currentStory);
            GameManager.ChangeState<MoveState>();
        }
        
        private void ContinueStory()
        {
            if (!_currentStory.canContinue)
            {
                ExitDialogue();
                return;
            }
            _nameBox.SetActive(false);
            
            string rawLine = _currentStory.Continue();
            string nextLine = InkHandlers.HandleNames(rawLine, _nameBox, _nameText);

            if (nextLine.Equals("") && !_currentStory.canContinue)
            {
                ExitDialogue();
                return;
            }
            
            _dialogueText.text = nextLine;
            
            InkHandlers.HandleTags(_currentStory.currentTags);
        }
        
        private void OnButtonClick()
        {
            if (IsAwaitThought) AcceptThought();
            else ContinueStory();
        }

        public void DisplayThought(Thought thought)
        {
            string displayText = thought ? thought.ThoughtData.PromoText : ChooseText;
            _dialogueText.text = $"<color={GameColor.PARASITE}>{displayText}</color>";
        }
        
        private void AcceptThought()
        {
            Thought thought = BrainManager.Instance.ChosenThought;
            if (!thought) return;
            
            TextAsset dialogue = thought.ThoughtData.TriggeredDialogue;
            
            if (dialogue) EnterDialogue(dialogue);
            else ExitDialogue();
            
            BrainManager.Instance.DestroyThought(thought);
            
            ThoughtChooseArea.SetMouth(false);
        }
    }
}