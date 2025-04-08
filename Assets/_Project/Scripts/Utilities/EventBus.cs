using System;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Neuromorph.Utilities
{
    public class EventBus : Singleton<EventBus>
    {
        //public static event Action<Entity> OnPlayerCreated;
        public static event Action<Vector2Int, int> OnPlayerMoved;
        public static event Action OnPlayerDied;
        public static event Action<string, Color> OnMessage;
        
        /*public static void SendPlayerCreated(Entity entity)
            => OnPlayerCreated?.Invoke(entity);*/
        public static void SendPlayerMoved(Vector2Int pos, int viewDistance)
            => OnPlayerMoved?.Invoke(pos, viewDistance);
        public static void SendPlayerDied()
            => OnPlayerDied?.Invoke();
        public static void SendMessage(string text, Color color)
            => OnMessage?.Invoke(text, color);
    }
}
