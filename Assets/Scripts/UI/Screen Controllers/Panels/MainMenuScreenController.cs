using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class MainMenuScreenController : APanelScreenController
    {
        [Header("Buttons")]
        [SerializeField] private CallbackButton playButton = null;
        [SerializeField] private CallbackButton creditsButton = null;
        [SerializeField] private CallbackButton quitButton = null;

        private void Awake()
        {
            playButton.Initialize(OnPlayButton);   
            creditsButton.Initialize(OnCreditsButton);   
            quitButton.Initialize(OnQuitButton);   
        }

        public override void Show(params object[] values)
        {
            base.Show(values);
            GameManager.Instance.ChangeGameState(GameState.MainMenu);
        }

        private void OnPlayButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.LEVEL_SELECTION_SCREEN, true);
            Hide();
        }

        private void OnCreditsButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.CREDITS_SCREEN, true);
            Hide();
        }

        private void OnQuitButton()
        {
            Application.Quit();
        }
    }
}