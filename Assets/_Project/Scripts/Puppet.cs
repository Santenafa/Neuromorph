using Neuromorph.Components;
using UnityEngine;

namespace Neuromorph
{
    public class Puppet: MonoBehaviour
    {
        public MovementComponent Movement { get; private set; }
        public PossessionComponent Possession { get; private set; }
        public CharacterStatsSO Stats => _stats;
        [SerializeField] private CharacterStatsSO _stats;

        private void OnEnable()
        {
            Movement = GetComponent<MovementComponent>();
            Possession = GetComponent<PossessionComponent>();
        }
    }
}