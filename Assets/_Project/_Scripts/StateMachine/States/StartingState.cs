namespace Neuromorph
{
public class StartingState : GameState
{
    public override void OnEnter()
    {
        StateMachine.ChangeState<MoveState>();
    }
}
}