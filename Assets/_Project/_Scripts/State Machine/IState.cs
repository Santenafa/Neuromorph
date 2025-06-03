namespace Neuromorph
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnFixedUpdate();
        void OnExit();
    }
    
    public abstract class BaseState : IState
    {
        public virtual void OnEnter(){}

        public virtual void OnUpdate(){}
        public virtual void OnFixedUpdate(){}
        public virtual void OnExit(){}
    }
}