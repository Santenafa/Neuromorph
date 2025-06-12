using System;
using UnityEngine;

namespace Neuromorph
{
    public class EventBus : Singleton<EventBus>
    {
        public static event Action<Vector2Int, int> OnPlayerMoved;
        public static event Action OnPlayerDied;
        public static void SendPlayerMoved(Vector2Int pos, int viewDistance)
            => OnPlayerMoved?.Invoke(pos, viewDistance);
        public static void SendPlayerDied()
            => OnPlayerDied?.Invoke();
    }
}
