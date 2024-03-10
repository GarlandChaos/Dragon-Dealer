using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Game.Data;

namespace Game.Gameplay
{
    public class CardVisualController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Vector3 initialPosition = Vector3.zero;
        private Vector3 dragOffset = Vector3.zero;
        private Card card;
        private ICardDropHandler raycastedDropHandler = null;
        private CardSlot cardSlot = null;
        private float dragScale = 0.3f;

        [Header("External Data")]
        [SerializeField] private ElementVisualData grassVisualData = null;
        [SerializeField] private ElementVisualData fireVisualData = null;
        [SerializeField] private ElementVisualData waterVisualData = null;

        [Header("Self Contained References")]
        [SerializeField] private RectTransform cardRectTransform = null;
        [SerializeField] private Image cardImage = null;
        [SerializeField] private TMP_Text cardValueText = null;

        public void Initialize(CardSlot cardSlot, Card card)
        {
            this.cardSlot = cardSlot;
            this.card = card;
            cardImage.sprite = GetCardSprite(card);
            cardValueText.SetText(card.value.ToString());
            cardRectTransform.anchoredPosition = Vector2.zero;
            transform.localScale = Vector3.one;
            StartCoroutine(SetInitialPositionRoutine());
        }

        public Sprite GetCardSprite(Card card)
        {
            return card.element switch
            {
                Element.GRASS => grassVisualData.CardSprite,
                Element.FIRE => fireVisualData.CardSprite,
                Element.WATER => waterVisualData.CardSprite,
                _ => grassVisualData.CardSprite
            };
        }

        public Color GetCardColor(Card card)
        {
            return card.element switch
            {
                Element.GRASS => grassVisualData.Color,
                Element.FIRE => fireVisualData.Color,
                Element.WATER => waterVisualData.Color,
                _ => grassVisualData.Color
            };
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 dragWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            dragOffset = dragWorldPos - transform.position;
            transform.localScale = Vector3.one * dragScale;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (raycastedDropHandler != null)
            {
                raycastedDropHandler.DropCard(LevelManager.Instance.PlayerController, card);
                raycastedDropHandler.ResetDropHandler();
                cardSlot.RemoveCard();
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