using UnityEngine;

namespace Neuromorph.Dialogues
{
    [RequireComponent(typeof(Collider2D))]
    public class ThoughtChooseArea: Singleton<ThoughtChooseArea>
    {
        private Collider2D _collider;
        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider2D>();
        }

        public static void SetMouth(bool isChoosing)
        {
            Instance._collider.enabled = isChoosing;
        }

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