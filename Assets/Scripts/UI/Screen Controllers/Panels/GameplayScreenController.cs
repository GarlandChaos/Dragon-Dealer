using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility.Pool;
using Game.Gameplay;

namespace Game.UI
{
    public class GameplayScreenController : APanelScreenController
    {
        //Runtime Data
        private Dictionary<EntityController, EntityUIController> entityUIControlerDictionary = new();

        [Header("Self Contained References")]
        [SerializeField] private EntityUIController playerUIControllerPrefab = null;
        [SerializeField] private EntityUIController playerUIController = null;
        [SerializeField] private EntityUIControllerPool entityUIControllerPool = null;
        [SerializeField] private Transform playerAreaTransform = null;
        [SerializeField] private Transform enemyAreaTransform = null;

        public override void Show(params object[] values)
        {
            LevelManager.Instance.onEnemyInstantiated += InstantiateEntityUI;
            base.Show(values);

            InitializePlayerUIController();

            LevelManager.Instance.ResetLevelIndex();
            LevelManager.Instance.InitializeLevel();
        }

        public override void Hide()
        {
            LevelManager.Instance.onEnemyInstantiated -= InstantiateEntityUI;
            base.Hide();
        }

        public void InitializePlayerUIController()
        {
            EntityController playerController = GameManager.Instance.PlayerController;
            if (playerController == null)
                playerController = GameManager.Instance.GetInitializedPlayer();


            if (playerUIController != null)
            {
                playerController.Initialize();
                playerUIController.Initialize(playerController);
                playerController.gameObject.SetActive(true);
                playerUIController.gameObject.SetActive(true);
                return;
            }
            
            InstantiateEntityUI(playerController);
        }

        private void InstantiateEntityUI(EntityController entityController)
        {
            entityController.HealthController.onEntityDead += OnEntityDead;

            EntityUIController entityUIController = 
                entityController.IsPlayer ? Instantiate(playerUIControllerPrefab) : entityUIControllerPool.Pool.Get();
            entityUIController.Initialize(entityController);

            if (entityController.IsPlayer)
                playerUIController = entityUIController;

            if(!entityUIControlerDictionary.ContainsKey(entityController))
                entityUIControlerDictionary.Add(entityController, entityUIController);

            Transform parentTransform = entityController.IsPlayer ? playerAreaTransform : enemyAreaTransform;
            entityUIController.transform.SetParent(parentTransform, false);
        }

        public void OnEntityDead(EntityController entityController)
        {
            entityController.HealthController.onEntityDead -= OnEntityDead;

            EntityUIController entityUIController = entityUIControlerDictionary[entityController];
            entityUIControllerPool.Pool.Release(entityUIController);
            entityUIControlerDictionary.Remove(entityController);

            foreach (KeyValuePair<EntityController, EntityUIController> kvp in entityUIControlerDictionary)
                kvp.Value.SetEntityBodyPosition();
        }
    }
}