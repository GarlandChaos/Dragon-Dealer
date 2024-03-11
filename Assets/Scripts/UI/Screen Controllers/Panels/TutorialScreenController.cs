using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Game.UI
{
    public class TutorialScreenController : APanelScreenController
    {
        [SerializeField] private CallbackButton playButton = null;
        private Action onPlayLevelCallback = null;

        private void Awake()
        {
            playButton.Initialize(OnPlayButton);
        }

        public override void Show(Action onCompleteCallback = null, params object[] values)
        {
            base.Show(onCompleteCallback, values);

            if (values.Length == 0) return;

            Action callback = values[0] as Action;
            if (callback == null) return;

            onPlayLevelCallback += callback;
        }

        private void OnPlayButton()
        {
            UIManager.Instance.RequestScreen(ScreenIds.GAMEPLAY_SCREEN, true);
            onPlayLevelCallback?.Invoke();
            Hide();
        }
    }
}