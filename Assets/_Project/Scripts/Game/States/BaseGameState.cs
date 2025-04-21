namespace Neuromorph
{
    public abstract class BaseGameState: BaseState
    {
        protected GameManager Game;

        public BaseGameState(GameManager game)
        {
            Game = game;
        }
    }
}