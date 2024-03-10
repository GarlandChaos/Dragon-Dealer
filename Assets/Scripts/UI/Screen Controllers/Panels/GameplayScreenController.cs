using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Gameplay;

namespace Game.UI
{
    public class GameplayScreenController : APanelScreenController
    {
        [Header("Self Contained References")]
        [SerializeField] private Transform playerAreaTransform = null;
        [SerializeField] private List<Transform> enemyPositionTransformList = null;
        [SerializeField] private TextSetter playerHealthTextSetter = null;
        [SerializeField] private CallbackButton pauseButton = null;

        private void Awake()
        {
            pauseButton.Initialize(OnPauseButton);
        }

        public override void Show(params object[] values)
        {
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
            base.Show(values);

            GameManager.Instance.ChangeGameState(GameState.WaveStart);
        }

        public override void Hide()
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
            base.Hide();
        }

        private void OnPauseButton()
        {
            Time.timeScale = 0f;
            GameManager.Instance.ChangeGameState(GameState.GamePause);
            UIManager.Instance.RequestScreen(ScreenIds.PAUSE_SCREEN, true);
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                case GameState.MainMenu:
                    break;
                case GameState.GameRunning:
                    LevelManager.Instance.PlayerController.EntityUIController.SetHealthTextSetter(playerHealthTextSetter);

                    int count = 0;
                    foreach (EntityUIController controller in EntityUIControllerManager.Instance.ActiveEntityUIControllerList)
                        controller.transform.SetParent(enemyPositionTransformList[count++], false);

                    LevelManager.Instance.PlayerController.EntityUIController.transform.SetParent(playerAreaTransform, false);
                    break;
                case GameState.GamePause:
                case GameState.GameEnd:
                    break;
                default:
                    break;
            }
        }
    }
}