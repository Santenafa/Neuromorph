using System;
using System.Collections.Generic;
using Neuromorph.Dialogues.Data;
using UnityEngine;

namespace Neuromorph.Dialogues
{
public class BrainManager: Singleton<BrainManager>
{
    [SerializeField] MemoryManager _memoryManager;
    [SerializeField] RecipesManager _recipesManager;
    public static event Action<Thought> OnChosenThought;
    public Thought ChosenThought
    {
        get => _chosenThought; private set
        {
            if (value == _chosenThought) return;
            
            _chosenThought?.SetState(ThoughtState.Idle);
            
            _chosenThought = value;
            OnChosenThought?.Invoke(_chosenThought);
        }
    }

    Thought _chosenThought;
    [SerializeField] Thought _thoughtPrefab;
    [SerializeField] Collider2D _spawnCollider;
    [SerializeField] Transform _spawnPoint;
    readonly List<Thought> _thoughtsInMouth = new();

    public bool TryFuse(string a, string b)
    {
        if (!_recipesManager.TryGetResults(a, b, out string[] results))
            return false;
        
        foreach (string result in results)
            SpawnThoughts(result);
        
        return true;
    }
    
    public void SpawnThoughts(string thoughts)
    {
        string[] splitThoughts = thoughts.Split(',');

        foreach (string thought in splitThoughts)
        {
            string clearThought = thought.Trim();
            
            Instantiate(_thoughtPrefab, _spawnPoint.position,
                Quaternion.identity, _spawnPoint
            ).Init(clearThought, _spawnCollider.bounds, this);
            
            _memoryManager.TryAddThoughts(clearThought);
        }
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
}}