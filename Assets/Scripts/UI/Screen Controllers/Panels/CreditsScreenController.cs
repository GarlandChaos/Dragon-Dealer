using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class CreditsScreenController : APanelScreenController
    {
        [SerializeField] private CallbackButton mainMenuButton = null;

        private void Awake()
        {
            mainMenuButton.Initialize(OnMainMenuButton);
        }

        private void OnMainMenuButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
            Hide();
        }
    }
}