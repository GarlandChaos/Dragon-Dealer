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
        [SerializeField] private CardVisualController cardVisualControllerPrefab = null;
        [SerializeField] private EntityController playerController = null;

        private List<CardVisualController> cardVisualControllerList = new();

        public EntityController PlayerController => playerController; //CHECK IF THERE IS NOT A BETTER PLACE TO HOLD THE PLAYER REFERENCE

        private void Start()
        {
            SetDeck();
        }

        public void SetDeck()
        {
            if (cardVisualControllerList.Count == deckSize) return;

            while (cardVisualControllerList.Count < deckSize)
            {
                Card card = DeckManager.Instance.GetCard();

                CardVisualController cardVisualController = Instantiate(cardVisualControllerPrefab, cardContainerTransform, false);
                cardVisualController.Initialize(this, card);
                cardVisualControllerList.Add(cardVisualController);
            }
        }

        public void RemoveCardFromDeck(CardVisualController cardVisualController)
        {
            cardVisualControllerList.Remove(cardVisualController);
            Destroy(cardVisualController.gameObject); //TODO: use object pool instead of destroying
            StartCoroutine(SetDeckCoroutine());
        }

        private IEnumerator SetDeckCoroutine()
        {
            yield return new WaitForSecondsRealtime(3f);
            SetDeck();
        }
    }
}