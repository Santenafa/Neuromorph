using UnityEngine;

namespace Neuromorph
{
public abstract class Interactable : MonoBehaviour
{
    public Vector3 InteractPoint => _interactPoint.position;
    [SerializeField] Transform _interactPoint;
    
    public abstract void Interact();
}}