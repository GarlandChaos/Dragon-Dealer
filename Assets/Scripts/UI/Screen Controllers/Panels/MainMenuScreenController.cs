using System.Collections;
using System.Collections.Generic;
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
        
        private void OnPlayButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.GAMEPLAY_SCREEN, true);
            Hide();
        }

        private void OnCreditsButton()
        {

        }

        private void OnQuitButton()
        {
            Application.Quit();
        }
    }
}