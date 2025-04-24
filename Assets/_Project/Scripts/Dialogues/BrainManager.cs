using System.Collections.Generic;
using UnityEngine;
using Neuromorph.Utilities;

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

                if (_talkState.State == ConState.AwaitThought)
                {
                    value?.SetState(ThoughtState.Chosen);
                    _talkState.DisplayThought(_chosenThought);
                }
            }
        }
        private Thought _chosenThought;
        [SerializeField] private Thought _thoughtPrefab;
        [SerializeField] private Collider2D _spawnCollider;
        [SerializeField] private Transform _spawnPoint;
        private readonly List<Thought> _thoughtsInMouth = new();
        private TalkState _talkState;

        private void Start()
        {
            _talkState = GameManager.GetState<TalkState>();
        }

        public void SpawnThought(ThoughtSO data)
        {
            Instantiate(_thoughtPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint)
                .Init(data, _spawnCollider.bounds);
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