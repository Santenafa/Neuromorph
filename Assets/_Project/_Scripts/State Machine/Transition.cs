namespace Neuromorph
{
    public class Transition : ITransition
    {
        public IState ToState { get; }
        public IPredicate Condition { get; }

        public Transition(IState to, IPredicate condition) {
            ToState = to;
            Condition = condition;
        }
    }
}