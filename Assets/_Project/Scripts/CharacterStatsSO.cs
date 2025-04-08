using UnityEngine;

namespace Neuromorph
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/CharacterStats", order = 1)]
    public class CharacterStatsSO : ScriptableObject
    {
        [Header("---------- Walk ----------")][Tooltip("")]
        public float WalkSpeed = 6f;
        
        [Header("---------- Rotation ----------")]
        public float RotationSmoothTime = 0.05f; //0.1f
        
        [Header("---------- Jump ----------")]
        public float JumpPower = 1.0f;
        
        [Header("---------- Gravity ----------")]
        public float GravityMultiplier = 3.0f;
        
        [Header("---------- Interact ----------")]
        public float InteractRange = 10f;
    }
}
