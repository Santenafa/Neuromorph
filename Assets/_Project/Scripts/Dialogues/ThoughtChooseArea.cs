using UnityEngine;

namespace Neuromorph.Dialogues
{
    [RequireComponent(typeof(Collider2D))]
    public class ThoughtChooseArea: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Thought thought))
                BrainManager.Instance.AddInMouth(thought);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Thought thought))
                BrainManager.Instance?.RemoveFromMouth(thought);
        }
    }
}