using System.Collections.Generic;
using Neuromorph.Dialogues;
using Neuromorph.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Neuromorph.UI
{
    public class WordsManager: Singleton<WordsManager>
    {
        [SerializeField] private Transform _thoughtsSpawn;
        [SerializeField] private MenuThought _menuThoughtPrefab;
        private readonly HashSet<string> _thoughtsList = new();
        
        public bool TryAddMenuThought(ThoughtSO data)
        {
            // check for duplication
            if (!_thoughtsList.Add(data.NameValue)) return false;
            
            Instantiate(_menuThoughtPrefab, _thoughtsSpawn).Init(data);
            return true;
        }
    }
}