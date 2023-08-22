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
        private MainMenu mainMenu = null;
        private GameOverMenu gameOverMenu = null;

        [HideInInspector] public BannerView bannerAd = null;
        private bool bannerLoaded = false;
        [HideInInspector] public RewardedAd rewardedAd = null;
        [HideInInspector] public InterstitialAd interstitialAd = null;

        public override void Awake()
        {
            base.Awake();

            MobileAds.Initialize(initStatus =>
            {
                // SDK initialization is complete
            });
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.buildIndex)
            {
                case Constants.SCENE_INDEX_MAIN_MENU:
                    if (!mainMenu)
                        mainMenu = FindObjectOfType<MainMenu>();

                    RequestBanner();
                    RequestInterstitial();
                    break;
                case Constants.SCENE_INDEX_GAME:
                    if (!gameOverMenu)
                        gameOverMenu = FindObjectOfType<GameOverMenu>();

                    RequestRewarded();
                    bannerAd.Hide();
                    break;
            }
        }

        #region Banner, Intertitial and Reward load and events

        // Banner
        private void RequestBanner()
        {
            if (bannerAd != null)
            {
                bannerAd.Destroy();
                bannerAd = null;
                bannerLoaded = false;
            }
            
            if (!bannerLoaded)
            {
                bannerAd = new BannerView(Constants.GOOGLE_ADS_TEST_BANNER_ID, AdSize.Banner, AdPosition.Top);
                // Events
                bannerAd.OnBannerAdLoaded += HandleOnBannerLoaded;
                bannerAd.OnBannerAdLoadFailed += HandleOnBannerFailedToLoad;
            }

            AdRequest adRequest = AdRequestBuild();
            bannerAd?.LoadAd(adRequest);
        }

        private void HandleOnBannerLoaded()
        {
            bannerLoaded = true;
        }

        private void HandleOnBannerFailedToLoad(LoadAdError error)
        {
            Debug.LogError($"HandleOnBannerFailedToLoad event received with message: {error}");
        }

        // Reward
        private void RequestRewarded()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd.OnAdFullScreenContentOpened -= HandleOnRewardOpening;
                rewardedAd.OnAdFullScreenContentClosed -= HandleOnRewardClosed;
                rewardedAd = null;
            }

            AdRequest adRequest = new AdRequest();

            RewardedAd.Load(Constants.GOOGLE_ADS_TEST_REWARDED_ID, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardedAd = ad;
                    HandleOnRewardLoaded();
                });
        }

        public void Reward()
        {
            ShowRewardedAd();
        }

        private void ShowRewardedAd()
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {Heart}, amount: {1}.";

            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    HandleOnUserEarnedReward();
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }

        private void HandleOnRewardLoaded()
        {
            rewardedAd.OnAdFullScreenContentOpened += HandleOnRewardOpening;
            rewardedAd.OnAdFullScreenContentClosed += HandleOnRewardClosed;
            if (gameOverMenu)
                gameOverMenu.rewardButton.interactable = true;
        }

        private void HandleOnRewardOpening()
        {
            if (gameOverMenu)
                gameOverMenu.rewardButton.interactable = false;
        }

        private void HandleOnUserEarnedReward()
        {
            bannerAd.Hide();
            HeartManager.Instance.Heal();
            ScoreManager.Instance.ReloadTimerAndUI();
            gameOverMenu.DeactivateMenu();
            if (GameManager.Instance.gameStarted)
                GameManager.Instance.gameIsRunning = true;
        }

        private void HandleOnRewardClosed()
        {
            RequestRewarded();
        }

        // Interstitial
        private void RequestInterstitial()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd.OnAdFullScreenContentOpened  -= HandleOnInterstitialOpening;
                interstitialAd.OnAdFullScreenContentClosed  -= HandleOnInterstitialClosed;
                interstitialAd = null;
            }
            
            AdRequest adRequest = AdRequestBuild();
            
            InterstitialAd.Load(Constants.GOOGLE_ADS_TEST_INTERSTITIAL_ID, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    interstitialAd = ad;
                    HandleOnInterstitialLoaded();
                });
        }

        public void Interstitial()
        {
            ShowInterstitialAd();
        }
        
        private void ShowInterstitialAd()
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }

        private void HandleOnInterstitialLoaded()
        {
            interstitialAd.OnAdFullScreenContentOpened  += HandleOnInterstitialOpening;
            interstitialAd.OnAdFullScreenContentClosed  += HandleOnInterstitialClosed;
            if (mainMenu)
                mainMenu.intertitialButton.interactable = true;
        }

        private void HandleOnInterstitialOpening()
        {
            if (mainMenu)
                mainMenu.intertitialButton.interactable = false;
        }

        private void HandleOnInterstitialClosed()
        {
            RequestInterstitial();
        }

        #endregion

        private AdRequest AdRequestBuild()
        {
            return new AdRequest();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            bannerAd.OnBannerAdLoaded -= HandleOnBannerLoaded;
            bannerAd.OnBannerAdLoadFailed -= HandleOnBannerFailedToLoad;

            rewardedAd.OnAdFullScreenContentOpened -= HandleOnRewardOpening;
            rewardedAd.OnAdFullScreenContentClosed -= HandleOnRewardClosed;

            interstitialAd.OnAdFullScreenContentOpened -= HandleOnInterstitialOpening;
            interstitialAd.OnAdFullScreenContentClosed -= HandleOnInterstitialClosed;
        }
    }
}