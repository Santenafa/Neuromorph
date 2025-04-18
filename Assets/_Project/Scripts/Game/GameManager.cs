using System;
using Neuromorph.Utilities;
using UnityEngine;

namespace Neuromorph
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;

        public GameState State { get; private set; }
        
        public void ChangeState(GameState newState)
        {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            //State.HandleState();

            OnAfterStateChanged?.Invoke(newState);
            
            Debug.Log($"New state: {newState}");
        }
    }
}
