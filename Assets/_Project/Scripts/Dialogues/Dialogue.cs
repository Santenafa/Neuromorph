namespace Neuromorph.Dialogues
{
    [System.Serializable]
    public class Dialogue
    {
        public string Name;
        [UnityEngine.TextArea(3, 10)]
        public string[] Sentences;
        public ThoughtSO ThoughtToSpawn;
    }
}