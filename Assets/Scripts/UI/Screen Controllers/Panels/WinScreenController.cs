using Game.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class WinScreen : APanelScreenController
    {
        [Header("Buttons")]
        [SerializeField] private CallbackButton nextLevelButton = null;
        [SerializeField] private CallbackButton mainMenuButton = null;

        private void Awake()
        {
            nextLevelButton.Initialize(OnNextLevelButton);
            mainMenuButton.Initialize(OnMainMenuButton);
        }

        public override void Show(params object[] values)
        {
            bool showNextLevelButton = LevelManager.Instance.HasLevelsRemaining;
            nextLevelButton.gameObject.SetActive(showNextLevelButton);
            
            base.Show(values);
        }

        private void OnNextLevelButton()
        {
            LevelManager.Instance.GoToNextLevel();
        }

        private void OnMainMenuButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
            Hide();
        }
    }
}