using System;
using Neuromorph.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Neuromorph
{
    public class GameManager : Singleton<GameManager>
    {
        public BaseGameState CurrentState { get; private set; }
        public static Puppet Player => Instance._player;
        [SerializeField] private Puppet _player;
        [SerializeField] private BaseGameState[] _gameStates;
        public static event Action<BaseGameState> OnBeforeStateChanged;
        public static event Action<BaseGameState> OnAfterStateChanged;
        protected override void Awake()
        {
            base.Awake();
            CurrentState = GetState<StartingState>();
            CurrentState?.OnEnter();
        }

        public void Update()
        {
            CurrentState.OnUpdate();
            if (Keyboard.current.escapeKey.isPressed) Application.Quit();
        }

        public void FixedUpdate() => CurrentState.OnFixedUpdate();

        public static T ChangeState<T>()
        {
            var newState = GetState<T>() as BaseGameState;
            if (!newState || newState == Instance.CurrentState) return default;
            
            OnBeforeStateChanged?.Invoke(newState);
            
            Instance.CurrentState?.OnExit();
            Instance.CurrentState = newState;
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
        
        public static bool IsPlayer(Collider other)
        {
            return other.TryGetComponent(out Puppet puppet)  && puppet == Player;
        }
    }
}
