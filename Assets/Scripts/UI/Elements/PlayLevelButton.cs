using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Gameplay;

namespace Game.UI
{
    public class PlayLevelButton : MonoBehaviour
    {
        private int level = 0;
        private Action onPlayLevelCallback = null;
        [SerializeField] private CallbackButton playLevelButton = null;
        [SerializeField] private TextSetter levelTextSetter = null;

        private void Awake()
        {
            playLevelButton.Initialize(OnPlayLevelButton);
        }

        public void Initialize(int level, Action callback = null)
        {
            this.level = level;

            string levelText = (level + 1).ToString();
            levelTextSetter.SetText(levelText);

            if (callback == null) return;

            onPlayLevelCallback = callback;
        }

        private void OnPlayLevelButton()
        {
            LevelManager.Instance.SetCurrentLevel(level);
            UIManager.Instance.RequestScreen(ScreenIds.GAMEPLAY_SCREEN, true);

            onPlayLevelCallback?.Invoke();
        }
    }
}