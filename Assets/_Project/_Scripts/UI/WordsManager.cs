using System.Collections.Generic;
using UnityEngine;

namespace Neuromorph.UI
{
    public class WordsManager: Singleton<WordsManager>
    {
        [SerializeField] private Transform _thoughtsSpawn;
        [SerializeField] private MenuThought _menuThoughtPrefab;
        private readonly HashSet<string> _thoughtsList = new();
        
        public bool TryAddMenuThought(string thoughtName)
        {
            if (!_thoughtsList.Add(thoughtName)) return false; // check for duplication
            
            Instantiate(_menuThoughtPrefab, _thoughtsSpawn).Init(thoughtName);
            return true;
        }
    }
}