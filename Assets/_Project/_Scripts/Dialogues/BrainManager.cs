using System.Collections.Generic;
using UnityEngine;

namespace Neuromorph.Dialogues
{
    public class BrainManager: Singleton<BrainManager>
    {
        public Thought ChosenThought
        {
            get => _chosenThought;
            private set
            {
                _chosenThought?.SetState(ThoughtState.Idle);
                _chosenThought = value;
                
                if (_dialogueState.IsAwaitThought)
                {
                    _chosenThought?.SetState(ThoughtState.Chosen);
                    _dialogueState.DisplayThought(_chosenThought);
                }
            }
        }
        private Thought _chosenThought;
        [SerializeField] private Thought _thoughtPrefab;
        [SerializeField] private Collider2D _spawnCollider;
        [SerializeField] private Transform _spawnPoint;
        private readonly List<Thought> _thoughtsInMouth = new();
        private DialogueState _dialogueState;

        private void Start()
        {
            _dialogueState = GameManager.GetState<DialogueState>();
        }
        public void SpawnThought(string thought)
        {
            Instantiate(_thoughtPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint)
                .Init(thought, _spawnCollider.bounds);
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