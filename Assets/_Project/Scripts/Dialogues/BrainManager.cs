using UnityEngine;
using Neuromorph.Utilities;

namespace Neuromorph.Dialogues
{
    public class BrainManager: Singleton<BrainManager>
    {
        public Thought ChosenThought
        {
            get => _chosenThought; set {
                _chosenThought = value;
                DialogueManager dialogueManager = DialogueManager.Instance;
                if (dialogueManager.State == DialogueManager.DialogueState.AwaitThought)
                    dialogueManager.DisplayThought(_chosenThought);
            }
        }
        private Thought _chosenThought;
        [SerializeField] private Thought _thoughtPrefab;
        [SerializeField] private Collider2D _spawnCollider;
        [SerializeField] private Transform _spawnPoint;
        private Bounds SpawnBounds => _spawnCollider.bounds;

        public void SpawnThought(ThoughtSO data)
        {
            Thought thought = Instantiate(_thoughtPrefab, _spawnPoint.position,
                Quaternion.identity, _spawnPoint);
            
            Bounds thoughtBounds = thought.Collider.bounds;
            
            float x = Random.Range(SpawnBounds.min.x, SpawnBounds.max.x - thoughtBounds.size.x); 
            float y = Random.Range(SpawnBounds.min.y + thoughtBounds.size.y, SpawnBounds.max.y);
            thought.Init(data, new Vector2(x, y));
        }

        public void DestroyThought(Thought thought)
        {
            Destroy(thought.gameObject);
        }
    }
}