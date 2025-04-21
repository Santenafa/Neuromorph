using System;
using Neuromorph.Utilities;
using UnityEngine;

namespace Neuromorph
{
    public class GameManager : Singleton<GameManager>
    {
        public IState State { get; private set; }
        public Puppet Player;
        public static event Action<IState> OnBeforeStateChanged;
        public static event Action<IState> OnAfterStateChanged;
        private StateMachine _stateMachine;
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine();
            var freeMoveState = new FreeMoveState(this);
            var inDialogueState = new InDialogueState(this);

            AddTran(inDialogueState, freeMoveState, new FuncPredicate(() => true));
            AddTran(freeMoveState, inDialogueState, new FuncPredicate(() => true));
            
            _stateMachine.SetInitialState(freeMoveState);
        }

        private void AddTran(IState from, IState to, IPredicate condition)
            => _stateMachine.AddTransition(from, to, condition);
        private void AddAnyTran(IState to, IPredicate condition)
            => _stateMachine.AddAnyTransition(to, condition);

        public void ChangeState(IState newState)
        {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            //State.HandleState();

            OnAfterStateChanged?.Invoke(newState);
            
            Debug.Log($"New state: {newState}");
        }
        
    }
}
