using Ink.Runtime;

namespace Neuromorph.Dialogues
{
    public static class InkExternalFunctions
    {
        // -------- Constants --------
        private const string SPAWN_THOUGHTS = "spawnThoughts";
        
        
        public static void Bind(Story story, DialogueState dialogueState)
        {
            story?.BindExternalFunction(SPAWN_THOUGHTS, 
                (string thoughtName) => BrainManager.Instance.SpawnThoughts(thoughtName));
        }
        
        public static void Unbind(Story story)
        {
            string[] funcToUnbind = {SPAWN_THOUGHTS};
            
            foreach (string func in funcToUnbind)
                story?.UnbindExternalFunction(func);
        }
    }
}