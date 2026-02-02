using System;
using Neuromorph.Dialogues;
using UnityEngine;

namespace Neuromorph
{
public static class EventBus
{
    public static Action<TextAsset> OnStartDialogue;
    public static Action<Thought> OnChosenThought;
}
}