using System.Collections.Generic;
using UnityEngine;

namespace Neuromorph.Dialogues.Data
{
public class MemoryManager: MonoBehaviour
{
    [SerializeField] Transform _thoughtsSpawn;
    [SerializeField] MenuThought _menuThoughtPrefab;
    readonly HashSet<string> _thoughtsList = new();
    
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
}}