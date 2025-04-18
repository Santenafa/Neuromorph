namespace Neuromorph
{
    public abstract class GameState
    {
        protected void HandleState(){}
    }
    public class StartingState : GameState
    {
        
    }
    public class WalkingState : GameState
    {
        
    }
    public class InDialogue : GameState
    {
        
    }
    public class Win : GameState
    {
        
    }
    public class Lose : GameState
    {
        
    }
}