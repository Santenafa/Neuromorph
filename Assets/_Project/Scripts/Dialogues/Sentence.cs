namespace Neuromorph.Dialogues
{
    [System.Serializable]
    public struct Sentence
    {
        [UnityEngine.TextArea(3, 10)]
        public string Text;
        public Events.DialogueEvent DialogueEvent;
    }
}