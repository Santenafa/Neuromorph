using UnityEngine;

namespace Neuromorph.Dialogues
{
    [CreateAssetMenu(fileName = "ThoughtData", menuName = "ScriptableObjects/ThoughtData", order = 2)]
    public class ThoughtSO : ScriptableObject
    {
        [Header("---------- Data ----------")][Tooltip("")]
        public string TextValue = "Nothing";
        
        public string Dialogue = "Nothing";
    }
}