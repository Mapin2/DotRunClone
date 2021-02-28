using UnityEngine;

namespace DotRun.Utils
{
    public static class Constants
    {
        // Playerprefs
        public const string PLAYERPREF_CURRENT_MATERIAL = "CurrentMaterial";
        public const string PLAYERPREF_MAX_CURRENT_SCORE = "CurrentScore";
        public const string PLAYERPREF_AUDIO_MUTED = "Muted";

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
        public const string TAG_HEART = "Heart";
        public const string TAG_POWERUP_INDICATOR = "PowerUpIndicator";

        // Layers
        public const int LAYER_INTERACTABLE_DOT = 8;
        public const int LAYER_NOT_INTERACTABLE_DOT = 9;

        // Paths
        public const string RESOURCES_MATERIALS_FOLDER = "Materials/";

        // Ads ids
        public const string GOOGLE_ADS_TEST_APP_ID = "ca-app-pub-3940256099942544~3347511713";
        public const string GOOGLE_ADS_TEST_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
        public const string GOOGLE_ADS_TEST_REWARDED_ID = "ca-app-pub-3940256099942544/6300978117";
        public const string GOOGLE_ADS_TEST_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/6300978112";
        

        // Others
        public const string DEFAULT_CURRENT_MATERIAL = "Red";

        // Object Names
        public const string OBJECT_NAME_X2 = "X2";
        public const string OBJECT_NAME_X3 = "X3";
    }
}
