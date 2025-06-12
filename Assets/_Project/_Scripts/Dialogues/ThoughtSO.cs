using UnityEngine;

namespace Neuromorph.Dialogues
{
    [CreateAssetMenu(fileName = "ThoughtData", menuName = "ScriptableObjects/ThoughtData", order = 2)]
    public class ThoughtSO : ScriptableObject
    {
        [Header("---------- Data ----------")][Tooltip("")]
        public string NameValue = "Nothing";
        public string PromoText = "I want nothing...";
        
        public TextAsset TriggeredDialogue;
    }
}