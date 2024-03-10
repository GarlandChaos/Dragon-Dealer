using Game.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.UI
{
    public class WaveTitleScreenController : ADialogScreenController
    {
        [SerializeField] private TextSetter waveTitleTextSetter = null;
        [SerializeField] private Transform levelIndicatorImageTransform = null;
        [SerializeField] private Transform levelIndicatorInitialPositionTransform = null;
        [SerializeField] private Transform levelIndicatorMiddlePositionTransform = null;
        [SerializeField] private Transform levelIndicatorFinalPositionTransform = null;
        private const string titleDivider = "-";
        private float titleDuration = 2f;
        private Tween moveLevelIndicatorTween = null;

        public override void Show(params object[] values)
        {
            int level = LevelManager.Instance.CurrentLevel + 1;
            int wave = LevelManager.Instance.CurrentWave + 1;
            string titleText = level.ToString() + titleDivider + wave.ToString();
            waveTitleTextSetter.SetText(titleText);
            base.Show(values);
            StartCoroutine(HideRoutine());
        }

        public override void Hide()
        {
            GameManager.Instance.ChangeGameState(GameState.GameRunning);
            base.Hide();
        }

        private IEnumerator HideRoutine()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float timer = 0f;
            while(timer < titleDuration)
            {
                timer += Time.deltaTime;
                yield return wait;
            }

            Hide();
        }
    }
}