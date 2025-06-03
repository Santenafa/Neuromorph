using UnityEngine;

namespace Neuromorph
{
    public abstract class BaseGameState: MonoBehaviour, IState
    {
        public virtual void OnEnter(){}
        public virtual void OnUpdate(){}
        public virtual void OnFixedUpdate(){}
        public virtual void OnExit(){}
    }
}