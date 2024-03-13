using Game.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Game.UI
{
    public class WinScreen : APanelScreenController
    {
        //Object data
        private const string scoreStringKey = "points";

        [Header("Localize String Events")]
        [SerializeField] LocalizeStringEvent scoreLocalizeStringEvent = null;

        [Header("Text Setters")]
        [SerializeField] private TextSetter scoreTextSetter = null;

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

            int score = ScoreManager.Instance.GetLevelScore(LevelManager.Instance.CurrentLevel);
            SetScoreText(score);

            base.Show(values);
        }

        private void OnNextLevelButton()
        {
            LevelManager.Instance.GoToNextLevel();
            Hide();
        }

        private void OnMainMenuButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
            Hide();
        }

        private void SetScoreText(int score)
        {
            StringVariable scoreVariable = scoreLocalizeStringEvent.StringReference[scoreStringKey] as StringVariable;
            scoreVariable.Value = score.ToString();
            
            string scoreText = scoreLocalizeStringEvent.StringReference.GetLocalizedString();
            scoreTextSetter.SetText(scoreText);
        }
    }
}