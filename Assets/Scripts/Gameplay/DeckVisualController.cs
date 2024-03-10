using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class DeckVisualController : MonoBehaviour
    {
        [Header("Object Data")]
        [SerializeField] private int deckSize = 3;

        [Header("Self Contained References")]
        [SerializeField] private Transform cardContainerTransform = null;

        [Header("External References")]
        [SerializeField] private CardSlot cardSlotPrefab = null;

        private void Start()
        {
            SetDeck();
        }

        private void SetDeck()
        {
            for(int i = 0; i < deckSize; i++)
            {
                CardSlot cardSlot = Instantiate(cardSlotPrefab, cardContainerTransform, false);
                cardSlot.CreateCard();
            }
        }
    }
}