namespace Neuromorph.Dialogues
{
    [System.Serializable]
    public class Dialogue
    {
        public string Name;
        public Sentence[] Sentences;
        public bool IsAwaitingThought;
    }
}