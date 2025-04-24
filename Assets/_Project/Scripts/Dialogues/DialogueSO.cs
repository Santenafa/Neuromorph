using UnityEngine;

namespace Neuromorph.Dialogues
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueData", order = 3)]
    public class DialogueSO : ScriptableObject
    {
        public string Name;
        public Sentence[] Sentences;
        public bool IsAwaitingThought;
    }
}