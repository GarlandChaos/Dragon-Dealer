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

        private void Start()
        {
            LevelController.Instance.onEnemyInstantiated += InstantiateEntityUI;
        }

        private void OnDestroy()
        {
            LevelController.Instance.onEnemyInstantiated -= InstantiateEntityUI;
        }

        public override void Show(params object[] values)
        {
            base.Show(values);

            EntityController playerController = GameManager.Instance.PlayerController;
            if (GameManager.Instance.PlayerController == null)
                playerController = GameManager.Instance.InstantiatePlayer();

            InstantiateEntityUI(playerController);
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