using System;
using UnityEngine;

namespace Neuromorph
{
    public class GameManager : Singleton<GameManager>
    {
        public static Puppet Player => Instance._player;
        [SerializeField] private Puppet _player;
        [SerializeField] private BaseGameState[] _gameStates;
        public static event Action<BaseGameState> OnBeforeStateChanged;
        public static event Action<BaseGameState> OnAfterStateChanged;
        private BaseGameState _currentState;
        protected override void Awake()
        {
            base.Awake();
            _currentState = GetState<StartingState>();
            _currentState?.OnEnter();
        }

        public void Update() => _currentState.OnUpdate();
        public void FixedUpdate() => _currentState.OnFixedUpdate();

        public static T ChangeState<T>()
        {
            var newState = GetState<T>() as BaseGameState;
            if (!newState || newState == Instance._currentState) return default;
            
            OnBeforeStateChanged?.Invoke(newState);
            
            Instance._currentState?.OnExit();
            Instance._currentState = newState;
            newState.OnEnter();

            OnAfterStateChanged?.Invoke(newState);
            Debug.Log($"New state: {newState}");
            
            return newState is T requiredState ? requiredState : default;
        }

        public static T GetState<T>()
        {
            foreach (BaseGameState state in Instance._gameStates) {
                if (state is T requiredState) return requiredState;
            }
            return default;
        }

        public static bool IsCurrentState<T>()
        {
            return Instance._currentState is T;
        }
        public static bool IsPlayer(Collider other)
        {
            return other.TryGetComponent(out Puppet puppet)  && puppet == Player;
        }
    }
}
