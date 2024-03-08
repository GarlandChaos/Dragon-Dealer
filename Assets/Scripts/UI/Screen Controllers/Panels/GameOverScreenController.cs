using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class GameOverScreenController : APanelScreenController
    {
        [Header("Buttons")]
        [SerializeField] private CallbackButton restartLevelButton = null;
        [SerializeField] private CallbackButton mainMenuButton = null;

        private void Awake()
        {
            restartLevelButton.Initialize(OnRestartLevelButton);
            mainMenuButton.Initialize(OnMainMenuButton);
        }

        private void OnRestartLevelButton()
        {

        }

        private void OnMainMenuButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
        }
    }
}