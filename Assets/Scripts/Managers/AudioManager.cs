using UnityEngine;

namespace Game.Audio
{
    public class AudioManager : ASingleton<AudioManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicAudioSource = null;
        [SerializeField] private AudioSource sfxAudioSource = null;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip mainMenuAudioClip = null;
        [SerializeField] private AudioClip gameplayAudioClip = null;
        [SerializeField] private AudioClip hitNormalAudioClip = null;
        [SerializeField] private AudioClip hitEffectiveAudioClip = null;
        [SerializeField] private AudioClip healAudioClip = null;
        [SerializeField] private AudioClip dropCardAudioClip = null;

        public AudioClip MainMenuAudioClip => mainMenuAudioClip;
        public AudioClip GameplayAudioClip => gameplayAudioClip;
        public AudioClip HitNormalAudioClip => hitNormalAudioClip;
        public AudioClip HitEffectiveAudioClip => hitEffectiveAudioClip;
        public AudioClip HealAudioClip => healAudioClip;
        public AudioClip DropCardAudioClip => dropCardAudioClip;

        private void Start()
        {
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;

            if(GameManager.Instance.CurrentGameState != GameState.NotInitialized)
                OnGameStateChanged(GameManager.Instance.CurrentGameState);
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }

        public void PlayMusic(AudioClip clip)
        {
            if (musicAudioSource.clip == clip) return;

            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }

        public void PlaySFX(AudioClip clip)
        {
            sfxAudioSource.clip = clip;
            sfxAudioSource.Play();
        }

        public void StopSFX()
        {
            sfxAudioSource.Stop();
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                case GameState.MainMenu:
                    PlayMusic(mainMenuAudioClip);
                    break;
                case GameState.WaveStart:
                    PlayMusic(gameplayAudioClip);
                    break;
                case GameState.GameRunning:
                case GameState.GamePause:
                    break;
                case GameState.GameEnd:
                    break;
                default:
                    break;
            }
        }
    }
}