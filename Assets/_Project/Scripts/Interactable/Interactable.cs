using UnityEngine;

namespace Neuromorph
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public Vector3 InteractPoint => _interactPoint.position;
        [SerializeField] private Transform _interactPoint;
        public abstract void Interact();
    }
}