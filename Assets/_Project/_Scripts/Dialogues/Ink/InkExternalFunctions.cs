using Ink.Runtime;

namespace Neuromorph.Dialogues
{
    public static class InkExternalFunctions
    {
        // -------- Constants --------
        private const string SPAWN_THOUGHT_FUNC = "spawnThought";
        
        public static void Bind(Story story)
        {
            story.BindExternalFunction(SPAWN_THOUGHT_FUNC, 
                (string thoughtName) => BrainManager.Instance.SpawnThought(thoughtName));
        }
        
        public static void Unbind(Story story)
        {
            story.UnbindExternalFunction(SPAWN_THOUGHT_FUNC);
        }
    }
}