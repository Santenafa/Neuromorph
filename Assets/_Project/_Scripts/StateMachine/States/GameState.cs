using UnityEngine;

namespace Neuromorph
{
public abstract class GameState: MonoBehaviour, IState
{
    protected StateMachine StateMachine;
    public void Init(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    public virtual void OnEnter(){}
    public virtual void OnUpdate(){}
    public virtual void OnFixedUpdate(){}
    public virtual void OnExit(){}
}
}