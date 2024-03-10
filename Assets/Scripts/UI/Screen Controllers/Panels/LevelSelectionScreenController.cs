using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility.Pool;
using Game.Gameplay;

namespace Game.UI
{
    public class LevelSelectionScreenController : APanelScreenController
    {
        [SerializeField] private PlayLevelButtonPool playLevelButtonPool = null;
        [SerializeField] private Transform buttonContainerTransform = null;
        [SerializeField] private CallbackButton mainMenuButton = null;
        private bool initialized = false;

        private void Awake()
        {
            mainMenuButton.Initialize(OnMainMenuButton);
        }

        public override void Show(params object[] values)
        {
            Initialize();
            base.Show(values);
        }

        private void Initialize()
        {
            if (initialized) return;

            int levelCount = LevelManager.Instance.NumberOfLevels;
            for (int i = 0; i < levelCount; i++)
            {
                PlayLevelButton playLevelButton = playLevelButtonPool.Pool.Get();
                playLevelButton.Initialize(i, () => Hide());
                playLevelButton.transform.SetParent(buttonContainerTransform, false);
            }

            initialized = true;
        }

        private void OnMainMenuButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
            Hide();
        }
    }
}