using System.Collections.Generic;
using UnityEngine;
using Game.Utility.Pool;
using Game.UI;

namespace Game.Gameplay
{
    public class DamageNumberElementManager : ASingleton<DamageNumberElementManager>
    {
        [SerializeField] private DamageNumberElementPool damageNumberElementPool = null;
        private List<DamageNumberElement> activeDamageNumberElementList = new();

        //Properties
        public int ActiveEntityControllerCount => activeDamageNumberElementList.Count;

        private void Start()
        {
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }

        public DamageNumberElement GetDamageNumberElement()
        {
            DamageNumberElement damageNumberElement = damageNumberElementPool.Pool.Get();
            damageNumberElement.transform.SetParent(UIManager.Instance.CanvasUI.transform, false);
            activeDamageNumberElementList.Add(damageNumberElement);
            
            return damageNumberElement;
        }

        public void ReleaseDamageNumberElement(DamageNumberElement damageNumberElement)
        {
            damageNumberElement.transform.SetParent(damageNumberElementPool.transform, false);
            activeDamageNumberElementList.Remove(damageNumberElement);
            damageNumberElementPool.Pool.Release(damageNumberElement);
        }

        public void ClearDamageNumberElementList()
        {
            foreach (DamageNumberElement damageNumberElement in activeDamageNumberElementList)
            {
                damageNumberElement.transform.SetParent(damageNumberElementPool.transform, false);
                damageNumberElementPool.Pool.Release(damageNumberElement);
            }

            activeDamageNumberElementList.Clear();
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                case GameState.MainMenu:
                case GameState.WaveStart:
                case GameState.GameRunning:
                case GameState.GamePause:
                case GameState.GameEnd:
                    ClearDamageNumberElementList();
                    break;
                default:
                    break;
            }
        }
    }
}