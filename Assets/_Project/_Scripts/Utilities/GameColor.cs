using UnityEngine;

namespace Neuromorph
{
    public static class GameColor
    {
        public static readonly Color PLAYER_ATTACK = new (.88f,.88f,.88f);
        public static readonly Color ENEMY_ATTACK = new (1,.75f,.75f);
        public static readonly Color PLAYER_DIE = new (1,0.19f,0.19f);
        public static readonly Color ENEMY_DIE = new (1,0.63f,0.19f);
        public static readonly Color WELCOME_TEXT = new (0.13f,0.63f,1);

        public static readonly Color INVALID = new (1,1,0);
        public static readonly Color IMPOSSIBLE = new (.5f,.5f,.5f);
        public static readonly Color ERROR = new (1,.25f,.25f);
        public static readonly Color HEALTH_RECOVERED = new (0,1,0);

        public static readonly Color STATUS_EFFECT_APPLIED = new (.25f,1,.25f);
        public static readonly Color DESCEND = new (.62f,.25f,1);
        
        public static readonly string BRAIN = "#c32454";
        public static readonly string BODY_BLACK = "#6e2727";
        public static readonly string PARASITE = "#239063";
    }
}