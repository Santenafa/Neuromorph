using System;
using UnityEngine;

namespace Neuromorph
{
public class GameManager : MonoBehaviour
{
    public Puppet Player => _player;
    [SerializeField] Puppet _player;
    [SerializeField] BaseGameState[] _gameStates;
    public static event Action<BaseGameState> OnBeforeStateChanged;
    public static event Action<BaseGameState> OnAfterStateChanged;
    BaseGameState _currentState;
    void Awake()
    {
        foreach (BaseGameState state in _gameStates)
            state.Init(this);
        
        _currentState = GetState<StartingState>();
        _currentState?.OnEnter();
    }

    public void Update() => _currentState.OnUpdate();
    public void FixedUpdate() => _currentState.OnFixedUpdate();

    public T ChangeState<T>()
    {
        var newState = GetState<T>() as BaseGameState;
        if (!newState || newState == _currentState) return default;
        
        OnBeforeStateChanged?.Invoke(newState);
        
        _currentState?.OnExit();
        _currentState = newState;
        newState.OnEnter();

        OnAfterStateChanged?.Invoke(newState);
        Debug.Log($"New state: {newState}");
        
        return newState is T requiredState ? requiredState : default;
    }

    public T GetState<T>()
    {
        foreach (BaseGameState state in _gameStates) {
            if (state is T requiredState) return requiredState;
        }
        return default;
    }

    public bool IsCurrentState<T>()
    {
        return _currentState is T;
    }
}}
