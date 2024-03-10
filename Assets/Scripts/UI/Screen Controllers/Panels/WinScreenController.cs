using Game.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class WinScreen : APanelScreenController
    {
        //Object data
        private const string scoreTextLabel = "Pontuação: ";

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
            string scoreText = scoreTextLabel + score.ToString();
            scoreTextSetter.SetText(scoreText);
        }
    }
}