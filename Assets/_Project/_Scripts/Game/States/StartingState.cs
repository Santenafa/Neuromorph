namespace Neuromorph
{
    public class StartingState : BaseGameState
    {
        public override void OnEnter()
        {
            GameManager.ChangeState<MoveState>();
        }
    }
}