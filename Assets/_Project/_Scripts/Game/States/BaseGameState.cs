using UnityEngine;

namespace Neuromorph
{
public abstract class BaseGameState: MonoBehaviour, IState
{
    protected GameManager GameManager;
    public void Init(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    public virtual void OnEnter(){}
    public virtual void OnUpdate(){}
    public virtual void OnFixedUpdate(){}
    public virtual void OnExit(){}
}}