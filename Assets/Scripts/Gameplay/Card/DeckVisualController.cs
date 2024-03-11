using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class DeckVisualController : MonoBehaviour
    {
        //Runtime Data
        private List<CardSlot> cardSlotList = new();
        private bool initialized = false;

        [Header("Object Data")]
        [SerializeField] private int deckSize = 3;

        [Header("Self Contained References")]
        [SerializeField] private Transform cardContainerTransform = null;

        [Header("External References")]
        [SerializeField] private CardSlot cardSlotPrefab = null;

        private void Start()
        {
            Initialize();
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }

        private void Initialize()
        {
            if (initialized) return;

            for(int i = 0; i < deckSize; i++)
            {
                CardSlot cardSlot = Instantiate(cardSlotPrefab, cardContainerTransform, false);
                cardSlot.gameObject.name = "Card Slot " + (i + 1).ToString();
                cardSlot.CreateCard();
                cardSlotList.Add(cardSlot);
            }

            initialized = true;
        }

        private void SetDeck()
        {
            Initialize();

            foreach(CardSlot cardSlot in cardSlotList)
                cardSlot.ResetCardSlot();
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.WaveStart:
                    SetDeck();
                    break;
                case GameState.NotInitialized:
                case GameState.MainMenu:
                case GameState.GameRunning:
                case GameState.GamePause:
                case GameState.GameEnd:                 
                default:
                    break;
            }
        }
    }
}