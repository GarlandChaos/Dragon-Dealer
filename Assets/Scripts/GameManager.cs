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
            LevelManager.Instance.onGameWon += OnGameWon;  
        }

        private void OnDestroy()
        {
            LevelManager.Instance.onGameWon -= OnGameWon;
        }

        public EntityController GetInitializedPlayer()
        {
            if(playerController == null)
                playerController = Instantiate(playerControllerPrefab);
            
            playerController.Initialize();
            return playerController;
        }

        private void OnGameWon()
        {
            playerController.gameObject.SetActive(false);
        }
    }
}