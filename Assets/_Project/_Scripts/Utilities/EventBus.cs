using System;
using UnityEngine;

namespace Neuromorph
{
public static class EventBus
{
    public static event Action OnPlayerDied;
    public static Action<TextAsset> TryStartDialogue;
    public static void SendPlayerDied() => OnPlayerDied?.Invoke();
}}
