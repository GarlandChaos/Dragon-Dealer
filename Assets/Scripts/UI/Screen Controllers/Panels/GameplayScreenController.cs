using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility.Pool;
using Game.Gameplay;

namespace Game.UI
{
    public class GameplayScreenController : APanelScreenController
    {
        [Header("Self Contained References")]
        [SerializeField] private EntityUIControllerPool entityUIControllerPool = null;
        [SerializeField] private Transform playerAreaTransform = null;
        [SerializeField] private Transform enemyAreaTransform = null;

        public override void Show(params object[] values)
        {
            LevelManager.Instance.onEnemyInstantiated += InstantiateEntityUI;
            base.Show(values);

            EntityController playerController = GameManager.Instance.PlayerController;
            if (GameManager.Instance.PlayerController == null)
                playerController = GameManager.Instance.InstantiatePlayer();

            InstantiateEntityUI(playerController);
            LevelManager.Instance.InitializeLevel();
        }

        public override void Hide()
        {
            LevelManager.Instance.onEnemyInstantiated -= InstantiateEntityUI;
            base.Hide();
        }

        private void InstantiateEntityUI(EntityController entityController)
        {
            EntityUIController entityUIController = entityUIControllerPool.Pool.Get();
            entityUIController.Initialize(entityController);

            Transform parentTransform = entityController.IsPlayer ? playerAreaTransform : enemyAreaTransform;
            entityUIController.transform.SetParent(parentTransform, false);
        }
    }
}