using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Game.Gameplay;

namespace Game
{
    public class GameManager : ASingleton<GameManager>
    {
        [SerializeField] private EntityController playerControllerPrefab = null;
        private EntityController playerController = null;

        //Properties
        public EntityController PlayerController => playerController;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            LevelController.Instance.InitializeLevel();
        }

        public EntityController InstantiatePlayer()
        {
            playerController = Instantiate(playerControllerPrefab);
            return playerController;
        }
    }
}