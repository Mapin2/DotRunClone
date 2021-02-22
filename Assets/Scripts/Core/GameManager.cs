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

        public int lifes = 3;

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
        }

        private void Start()
        {
            mapGenerator.GenerateMap();
        }

        public void Harm()
        {
            lifes--;
            // score - 5 points
            if (lifes <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            throw new NotImplementedException();
        }
    }
}
