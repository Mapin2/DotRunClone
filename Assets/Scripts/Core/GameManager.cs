using UnityEngine;
using DotRun.Utils;
using DotRun.GamePlay;
using System;

namespace DotRun.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public Material currentMaterial = null;
        [SerializeField] private Material lastCurrentMaterial = null;
        [SerializeField] private MapGenerator mapGenerator = null;
        [SerializeField] private HeartManager heartManager = null;

        public override void Awake()
        {
            // Call singleton awake
            base.Awake();

            // Load material saved on playerprefs if exist
            string currenMaterialName = PlayerPrefs.GetString(Constants.PLAYERPREF_CURRENT_MATERIAL, Constants.DEFAULT_CURRENT_MATERIAL);
            lastCurrentMaterial = Resources.Load(Constants.RESOURCES_MATERIALS_FOLDER + currenMaterialName, typeof(Material)) as Material;

            // The current color will be used for some UI and generation of the map based on this material
            currentMaterial = lastCurrentMaterial;
            lastCurrentMaterial = null;

            if (!mapGenerator)
                mapGenerator = FindObjectOfType<MapGenerator>();

            if (!heartManager)
                heartManager = FindObjectOfType<HeartManager>();
        }

        private void Start()
        {
            mapGenerator.GenerateMap();
        }

        public void Hurt()
        {
            heartManager.Hurt();
            if (!heartManager.isAlive)
                GameOver();
        }

        public void Heal()
        {
            heartManager.Heal();
        }

        private void GameOver()
        {
            throw new NotImplementedException();
        }
    }
}
