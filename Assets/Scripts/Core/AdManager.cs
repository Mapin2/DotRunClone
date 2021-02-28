using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DotRun.Utils;
using DotRun.Menu;
using GoogleMobileAds.Api;

namespace DotRun.Core
{
    public class AdManager : Singleton<AdManager>
    {
        private GameOverMenu gameOverMenu = null;

        [HideInInspector] public BannerView bannerAd = null;
        [HideInInspector] public RewardedAd rewardedAd = null;
        private bool rewardLoaded = false;
        [HideInInspector] public InterstitialAd interstitialAd = null;
        private bool intertitialLoaded = false;

        private MainMenu mainMenu = null;

        public override void Awake()
        {
            base.Awake();

            MobileAds.Initialize((initStatus) =>
            {
                // SDK initialization is complete
            });
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == Constants.SCENE_INDEX_MAIN_MENU)
            {
                if (!mainMenu)
                    mainMenu = FindObjectOfType<MainMenu>();  
                
                RequestBanner();
                RequestIntertitial();
            }

            if (scene.buildIndex == Constants.SCENE_INDEX_GAME)
            {
                if (!gameOverMenu)
                    gameOverMenu = FindObjectOfType<GameOverMenu>();

                RequestRewarded();

                bannerAd.Hide();
            }
        }

        private void RequestBanner()
        {
            bannerAd = new BannerView(Constants.GOOGLE_ADS_TEST_BANNER_ID, AdSize.Banner, AdPosition.Top);
            AdRequest request = AdRequestBuild();
            bannerAd.LoadAd(request);
        }

        #region Intertitial and Reward
        // Reward
        private void RequestRewarded()
        {
            if (!rewardLoaded)
            {
                rewardedAd = new RewardedAd(Constants.GOOGLE_ADS_TEST_REWARDED_ID);
                // Events
                rewardedAd.OnAdLoaded += HandleOnRewardLoaded;
                rewardedAd.OnAdOpening += HandleOnRewardOpening;
                rewardedAd.OnUserEarnedReward += HandleOnUserEarnedReward;
                rewardedAd.OnAdClosed += HandleOnRewardClosed;
                rewardLoaded = true;
            }

            AdRequest request = AdRequestBuild();
            rewardedAd.LoadAd(request);
        }

        public void Reward()
        {
            if (rewardedAd.IsLoaded())
                rewardedAd.Show();
        }

        public void HandleOnRewardLoaded(object sender, EventArgs args)
        {
            gameOverMenu.rewardButton.interactable = true;
        }

        public void HandleOnRewardOpening(object sender, EventArgs args)
        {
            gameOverMenu.rewardButton.interactable = false;
        }

        public void HandleOnUserEarnedReward(object sender, Reward args)
        {
            Debug.Log("Rewarded");
            bannerAd.Hide();
            HeartManager.Instance.Heal();
            gameOverMenu.DeactivateMenu();
            if (GameManager.Instance.gameStarted)
            {
                GameManager.Instance.gameIsRunning = true;
            }
        }

        public void HandleOnRewardClosed(object sender, EventArgs args)
        {
            RequestRewarded();
        }

        // Intertitial
        private void RequestIntertitial()
        {
            // Events
            if (!intertitialLoaded)
            {
                interstitialAd = new InterstitialAd(Constants.GOOGLE_ADS_TEST_INTERSTITIAL_ID);
                interstitialAd.OnAdLoaded += HandleOnIntertitialLoaded;
                interstitialAd.OnAdOpening += HandleOnIntertitialOpening;
                interstitialAd.OnAdClosed += HandleOnIntertitialClosed;
                intertitialLoaded = true;
            }

            AdRequest request = AdRequestBuild();
            interstitialAd.LoadAd(request);

        }

        public void Intertitial()
        {
            if (interstitialAd.IsLoaded())
                interstitialAd.Show();
        }

        public void HandleOnIntertitialLoaded(object sender, EventArgs args)
        {
            mainMenu.intertitialButton.interactable = true;
        }

        public void HandleOnIntertitialOpening(object sender, EventArgs args)
        {
            mainMenu.intertitialButton.interactable = false;
        }

        public void HandleOnIntertitialClosed(object sender, EventArgs args)
        {
            RequestIntertitial();
        }
        #endregion

        AdRequest AdRequestBuild()
        {
            return new AdRequest.Builder().Build();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
