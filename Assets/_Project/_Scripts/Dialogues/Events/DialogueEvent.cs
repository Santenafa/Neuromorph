namespace Neuromorph.Dialogues.Events
{
    [System.Serializable]
    public class DialogueEvent
    {
        public ThoughtSO ThoughtToSpawn;
        public void TrySpawnThought()
        {
            if (ThoughtToSpawn) BrainManager.Instance.SpawnThought(ThoughtToSpawn);
        }
    }
}