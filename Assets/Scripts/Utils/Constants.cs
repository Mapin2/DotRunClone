using UnityEngine;

namespace DotRun.Utils
{
    public static class Constants
    {
        // Playerprefs
        public const string PLAYERPREF_CURRENT_MATERIAL = "CurrentMaterial";
        public const string PLAYERPREF_CURRENT_SCORE = "CurrentScore";

        // Scene index
        public const int SCENE_INDEX_LOGO = 0;
        public const int SCENE_INDEX_MAIN_MENU = 1;
        public const int SCENE_INDEX_GAME = 2;

        // Audio
        //public const string AUDIO_X = "X";

        // Animations
        private const string ANIM_FADE_OUT = "Fade_Out_Trigger";

        // Animations hash ids
        public static readonly int ANIM_FADE_OUT_ID = Animator.StringToHash(ANIM_FADE_OUT);

        // Tags
        public const string TAG_DOT = "Dot";

        // Layers
        public const int LAYER_INTERACTABLE_DOT = 8;
        public const int LAYER_NOT_INTERACTABLE_DOT = 9;

        // Paths
        public const string RESOURCES_MATERIALS_FOLDER = "Materials/";

        // Others
        public const string DEFAULT_CURRENT_MATERIAL = "Red";
    }
}
