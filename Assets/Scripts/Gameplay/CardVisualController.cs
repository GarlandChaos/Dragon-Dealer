using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Gameplay
{
    public class CardVisualController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Vector3 initialPosition = Vector3.zero;
        private Vector3 dragOffset = Vector3.zero;
        private Card card;
        private ICardDropHandler raycastedDropHandler = null;
        private DeckVisualController deckVisualController = null; //TEMPORARY, USE EVENTS LATER TO DECOUPLE

        [Header("Object Data")]
        [SerializeField] private Color grassElementColor = Color.green;
        [SerializeField] private Color fireElementColor = Color.red;
        [SerializeField] private Color waterElementColor = Color.blue;

        [Header("Self Contained References")]
        [SerializeField] private Image cardImage = null;
        [SerializeField] private TMP_Text cardValueText = null;

        public void Initialize(DeckVisualController deckVisualController, Card card)
        {
            this.deckVisualController = deckVisualController;
            this.card = card;
            cardImage.color = GetCardColor(card);
            cardValueText.SetText(card.value.ToString());
            StartCoroutine(SetInitialPositionRoutine());
        }

        public Color GetCardColor(Card card)
        {
            return card.element switch
            {
                Element.GRASS => grassElementColor,
                Element.FIRE => fireElementColor,
                Element.WATER => waterElementColor,
                _ => grassElementColor
            };
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 dragWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            dragOffset = dragWorldPos - transform.position;
            transform.localScale = Vector3.one * 0.5f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (raycastedDropHandler != null)
            {
                raycastedDropHandler.DropCard(GameManager.Instance.PlayerController, card);
                raycastedDropHandler.ResetDropHandler();
                deckVisualController.RemoveCardFromDeck(this);
                return;
            }

            transform.position = initialPosition;
            transform.localScale = Vector3.one;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 dragWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            Vector3 newPos = dragWorldPos - dragOffset;
            newPos.z = Camera.main.nearClipPlane;
            transform.position = newPos;

            List<RaycastResult> raycastResults = new();
            EventSystem.current.RaycastAll(eventData, raycastResults);


            ICardDropHandler dropHandler = null;
            foreach(RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out dropHandler))
                {
                    raycastedDropHandler = dropHandler;
                    dropHandler.SetDropImageColor(GetCardColor(card));
                    break;
                }
            }

            if (dropHandler == null)
            {
                if (raycastedDropHandler == null) return;

                raycastedDropHandler.ResetDropHandler();
                raycastedDropHandler = null;
                return;
            }
        }

        private IEnumerator SetInitialPositionRoutine()
        {
            yield return new WaitForEndOfFrame();
            initialPosition = transform.position;
        }
    }
}