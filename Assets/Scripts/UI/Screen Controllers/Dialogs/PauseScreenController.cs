using UnityEngine;

namespace Game.UI
{
    public class PauseScreenController : ADialogScreenController
    {
        [SerializeField] private CallbackButton unpauseButton = null;
        [SerializeField] private CallbackButton restartLevelButton = null;
        [SerializeField] private CallbackButton mainMenuButton = null;
        [SerializeField] private CallbackButton closeButton = null;

        private void Awake()
        {
            unpauseButton.Initialize(OnUnpauseButton);
            restartLevelButton.Initialize(OnRestartLevelButton);
            mainMenuButton.Initialize(OnMainMenuButton);
            closeButton.Initialize(OnUnpauseButton);
        }

        private void OnUnpauseButton()
        {
            Time.timeScale = 1f;
            GameManager.Instance.ChangeGameState(GameState.GameRunning);
            Hide();
        }

        private void OnRestartLevelButton()
        {
            Time.timeScale = 1f;
            GameManager.Instance.ChangeGameState(GameState.GameEnd);
            GameManager.Instance.ChangeGameState(GameState.WaveStart);
            Hide();
        }

        private void OnMainMenuButton()
        {
            Time.timeScale = 1f;
            UIManager.Instance.RequestScreen(ScreenIds.GAMEPLAY_SCREEN, false);
            UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
            Hide();
        }
    }
}