using System.Collections;
using System.Collections.Generic;
using Neuromorph.Utilities;
using UnityEngine;
using TMPro;

namespace Neuromorph.Dialogues
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] private float _textSpeed;
        [SerializeField] private Canvas _dialogueCanvas;
        [SerializeField] private Animator _dialogueAnimator;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _dialogueText;
        
        private readonly Queue<string> _sentences = new();
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        
        public void StartDialogue(Dialogue dialogue)
        {
            _dialogueAnimator.SetBool(IsOpen, true);
            //_dialogueCanvas.gameObject.SetActive(true);
            _nameText.text = dialogue.Name;
            _sentences.Clear();

            foreach (string sentence in dialogue.Sentences)
                _sentences.Enqueue(sentence);
            
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0) {
                EndDialogue();
                return;
            }
            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
            //_dialogueText.text = sentence;
        }

        private IEnumerator TypeSentence(string sentence)
        {
            _dialogueText.text = "";
            foreach (char letter in sentence)
            {
                _dialogueText.text += letter;
                yield return new WaitForSeconds(_textSpeed);
            }
        }

        private void EndDialogue()
        {
            _dialogueAnimator.SetBool(IsOpen, false);
            //_dialogueCanvas.gameObject.SetActive(false);
        }
    }
}
