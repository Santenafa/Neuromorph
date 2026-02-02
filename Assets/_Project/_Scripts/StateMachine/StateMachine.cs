using UnityEngine;

namespace Neuromorph
{
public class StateMachine : MonoBehaviour
{
    [SerializeField] GameState[] _gameStates;
    GameState _currentState;
    
    void Awake()
    {
        foreach (GameState state in _gameStates)
            state.Init(this);
        
        _currentState = GetState<StartingState>();
        _currentState?.OnEnter();
    }

    public void Update() => _currentState.OnUpdate();
    public void FixedUpdate() => _currentState.OnFixedUpdate();
    
    public bool IsCurrentState<T>() => _currentState is T;

    public T ChangeState<T>()
    {
        var newState = GetState<T>() as GameState;
        if (!newState || newState == _currentState) return default;
        
        _currentState?.OnExit();
        _currentState = newState;
        newState.OnEnter();
        Debug.Log($"New state: {newState}");
        
        return newState is T requiredState ? requiredState : default;
    }

    T GetState<T>()
    {
        foreach (GameState state in _gameStates) {
            if (state is T requiredState) return requiredState;
        }
        return default;
    }
}
}
