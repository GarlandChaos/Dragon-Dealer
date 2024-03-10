using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay
{
    public class CardSlot : MonoBehaviour
    {
        [SerializeField] private Transform cardContainer = null;
        [SerializeField] private Image timerImage = null;
        private float createCardCooldownDuration = 3f;

        private void Awake()
        {
            timerImage.gameObject.SetActive(false);
        }

        public void CreateCard()
        {
            Card card = DeckManager.Instance.GetCard();
            CardVisualController cardVisualController = CardVisualControllerManager.Instance.GetCardVisualController();
            cardVisualController.Initialize(this, card);
            cardVisualController.transform.SetParent(cardContainer, false);
        }

        public void RemoveCard(CardVisualController cardVisualController)
        {
            CardVisualControllerManager.Instance.ReleaseCardVisualController(cardVisualController);
            StartCoroutine(CreateCardCooldownRoutine());
        }

        private IEnumerator CreateCardCooldownRoutine()
        {
            timerImage.gameObject.SetActive(true);
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float timer = createCardCooldownDuration;
            while (timer > 0f)
            {
                yield return wait;
                timer -= Time.deltaTime;
                timerImage.fillAmount = timer / createCardCooldownDuration;
            }
            timerImage.gameObject.SetActive(false);
            CreateCard();
        }
    }
}