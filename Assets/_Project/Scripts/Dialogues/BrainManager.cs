using System.Collections.Generic;
using UnityEngine;
using Neuromorph.Utilities;
using Random = UnityEngine.Random;

namespace Neuromorph.Dialogues
{
    public class BrainManager: Singleton<BrainManager>
    {
        public Thought ChosenThought
        {
            get => _chosenThought; private set {
                _chosenThought = value;
                if (_dialogueState.State == ConState.AwaitThought)
                    _dialogueState.DisplayThought(_chosenThought);
            }
        }
        private Thought _chosenThought;
        [SerializeField] private Thought _thoughtPrefab;
        [SerializeField] private Collider2D _spawnCollider;
        [SerializeField] private Transform _spawnPoint;
        private readonly List<Thought> _thoughtsInMouth = new();
        private Bounds SpawnBounds => _spawnCollider.bounds;
        private DialogueState _dialogueState;

        private void Start()
        {
            _dialogueState = GameManager.GetState<DialogueState>();
        }

        public void SpawnThought(ThoughtSO data)
        {
            Thought thought = Instantiate(_thoughtPrefab, _spawnPoint.position,
                Quaternion.identity, _spawnPoint);
            
            Bounds thoughtBounds = thought.Collider.bounds;
            
            float x = Random.Range(SpawnBounds.min.x, SpawnBounds.max.x - thoughtBounds.size.x); 
            float y = Random.Range(SpawnBounds.min.y + thoughtBounds.size.y, SpawnBounds.max.y);
            thought.Init(data, new Vector2(x, y));
        }

        public void AddInMouth(Thought thought)
        {
            _thoughtsInMouth.Add(thought);
            ChosenThought = _thoughtsInMouth[0];
        }
        
        public void RemoveFromMouth(Thought thought)
        {
            _thoughtsInMouth.Remove(thought);
            ChosenThought = _thoughtsInMouth.Count > 0 ? _thoughtsInMouth[0] : null;
        }

        public void DestroyThought(Thought thought)
        {
            RemoveFromMouth(thought);
            Destroy(thought.gameObject);
        }
    }
}