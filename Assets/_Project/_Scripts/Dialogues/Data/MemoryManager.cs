using System.Collections.Generic;
using UnityEngine;

namespace Neuromorph.Dialogues.Data
{
    public class MemoryManager: Singleton<MemoryManager>
    {
        [SerializeField] private Transform _thoughtsSpawn;
        [SerializeField] private MenuThought _menuThoughtPrefab;
        private readonly HashSet<string> _thoughtsList = new();
        
        
        public bool TryAddThoughts(string thought)
        {
            bool wasAdded = _thoughtsList.Add(thought.Trim()); // check for duplication

            if (wasAdded)
            {
                Instantiate(_menuThoughtPrefab, _thoughtsSpawn).Init(thought);
                //TODO: Add sound
            }
            
            return wasAdded;
        }
    }
}