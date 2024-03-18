using System.Collections.Generic;
using UnityEngine;
using Game.Gameplay;
using Game.Data;
using NaughtyAttributes;

namespace Game.UI
{
    public class GameplayScreenController : APanelScreenController
    {
        [Header("External Data")]
        [SerializeField] private ElementVisualData grassVisualData = null;
        [SerializeField] private ElementVisualData fireVisualData = null;
        [SerializeField] private ElementVisualData waterVisualData = null;

        [Header("Self Contained References")]
        [SerializeField] private Transform playerAreaTransform = null;
        [SerializeField] [ValidateInput("ValidateTransformListSize", "List size must be 4 or under")] private List<Transform> enemyPositionTransformList = null;
        [SerializeField] [ValidateInput("ValidateEnemyHealthElementListSize", "List size must be 4 or under")] private List<EnemyHealthElement> enemyHealthElementList = null;
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

        public Sprite GetHealthIconSprite(Element element)
        {
            return element switch
            {
                Element.GRASS => grassVisualData.HealthIconSprite,
                Element.FIRE => fireVisualData.HealthIconSprite,
                Element.WATER => waterVisualData.HealthIconSprite,
                _ => grassVisualData.HealthIconSprite
            };
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                case GameState.MainMenu:
                    break;
                case GameState.WaveStart:
                    foreach(EnemyHealthElement enemyHealthElement in enemyHealthElementList)
                        enemyHealthElement.DisableGraphics();
                    
                    break;
                case GameState.GameRunning:
                    LevelManager.Instance.PlayerController.EntityUIController.SetHealthTextSetter(playerHealthTextSetter);

                    int count = 0;
                    foreach (EntityUIController controller in EntityUIControllerManager.Instance.ActiveEntityUIControllerList)
                    {
                        EnemyHealthElement enemyHealthElement = enemyHealthElementList[count];
                        controller.transform.SetParent(enemyPositionTransformList[count], false);
                        controller.SetHealthTextSetter(enemyHealthElement.HealthTextSetter, enemyHealthElement);

                        Sprite healthIconSprite = GetHealthIconSprite(controller.EntityController.Element);
                        enemyHealthElement.SetHealthIconSprite(healthIconSprite);
                        enemyHealthElement.EnableGraphics();

                        count++;
                    }

                    if (count < enemyHealthElementList.Count)
                    {
                        for (int i = count; i < enemyHealthElementList.Count; i++)
                            enemyHealthElementList[i].DisableGraphics();
                    }

                    LevelManager.Instance.PlayerController.EntityUIController.transform.SetParent(playerAreaTransform, false);
                    break;
                case GameState.GamePause:
                case GameState.GameEnd:
                    break;
                default:
                    break;
            }
        }

#if UNITY_EDITOR
        private bool ValidateTransformListSize(List<Transform> list)
        {
            return list.Count <= 4;
        }

        private bool ValidateEnemyHealthElementListSize(List<EnemyHealthElement> list)
        {
            return list.Count <= 4;
        }
#endif
    }
}