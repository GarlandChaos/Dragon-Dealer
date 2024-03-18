using Game.Gameplay;
using UnityEngine;
using DG.Tweening;

namespace Game.UI
{
    public class WaveTitleScreenController : ADialogScreenController
    {
        //Object data
        private bool initialized = false;
        private const string titleDivider = "-";
        private float titleMoveDuration = 0.5f;
        private float titleStaticDuration = 2f;
        private Sequence moveSequence = null;

        [Header("Self Contained References")]
        [SerializeField] private TextSetter waveTitleTextSetter = null;
        [SerializeField] private Transform levelIndicatorImageTransform = null;
        [SerializeField] private Transform levelIndicatorInitialPositionTransform = null;
        [SerializeField] private Transform levelIndicatorMiddlePositionTransform = null;
        [SerializeField] private Transform levelIndicatorFinalPositionTransform = null;

        private void Initialize()
        {
            if(initialized) return;

            moveSequence = DOTween.Sequence();
            moveSequence.Append(levelIndicatorImageTransform.DOMoveX(levelIndicatorInitialPositionTransform.position.x, 0f));
            moveSequence.Append(levelIndicatorImageTransform.DOMoveX(levelIndicatorMiddlePositionTransform.position.x, titleMoveDuration));
            moveSequence.AppendInterval(titleStaticDuration);
            moveSequence.Append(levelIndicatorImageTransform.DOMoveX(levelIndicatorFinalPositionTransform.position.x, titleMoveDuration));
            moveSequence.onComplete += () => Hide();
            moveSequence.SetAutoKill(false);

            initialized = true;
        }

        public override void Show(params object[] values)
        {
            int level = LevelManager.Instance.CurrentLevel + 1;
            int wave = LevelManager.Instance.CurrentWave + 1;
            string titleText = level.ToString() + titleDivider + wave.ToString();
            waveTitleTextSetter.SetText(titleText);
            
            base.Show(values);
            
            Initialize();
            moveSequence.Restart();
        }

        public override void Hide()
        {
            GameManager.Instance.ChangeGameState(GameState.GameRunning);
            base.Hide();
        }
    }
}