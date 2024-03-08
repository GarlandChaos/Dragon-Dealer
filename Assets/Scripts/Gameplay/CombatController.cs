using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay
{
    public class CombatController : MonoBehaviour
    {
        [SerializeField] private EntityController entityController = null;
        private Element element = Element.NONE;

        [SerializeField] private bool useAttackTimer = false;
        private bool waitingToAttack = true;
        private float attackTimer = 0;
        [SerializeField] private float waitForAttackDuration = 0;
        [SerializeField] private Image attackTimerImage = null;
        [SerializeField] private GameObject cardDropAreaGameObject = null;

        private void Start()
        {
            CombatManager.Instance.onCombatPacketCreated += OnCombatPacketCreated;
            CombatManager.Instance.onCurrentCombatFinished += OnCurrentCombatFinished;
        }

        private void OnDestroy()
        {
            CombatManager.Instance.onCombatPacketCreated -= OnCombatPacketCreated;
            CombatManager.Instance.onCurrentCombatFinished -= OnCurrentCombatFinished;
        }

        private void Update()
        {
            if (!useAttackTimer) return;

            if (!waitingToAttack) return;

            attackTimer += Time.deltaTime;
            attackTimerImage.fillAmount = attackTimer / waitForAttackDuration;
            if(attackTimer >= waitForAttackDuration)
            {
                StopAttackWaiting();
                Card attackCard = new Card(element, Random.Range(5, 26));
                CombatManager.Instance.CreateCombatPacket(entityController, GameManager.Instance.PlayerController, attackCard);
            }
        }

        private void OnCombatPacketCreated(CombatPacket obj)
        {
            cardDropAreaGameObject.SetActive(false);
        }

        private void OnCurrentCombatFinished()
        {
            cardDropAreaGameObject.SetActive(true);
        }

        public void SetEntityElement(Element element)
        {
            this.element = element;
            entityController.BodyImage.color = GetElementColor(element);
        }

        public Color GetElementColor(Element element)
        {
            return element switch
            {
                Element.GRASS => Color.green,
                Element.FIRE => Color.red,
                Element.WATER => Color.blue,
                _ => Color.white
            };
        }

        public void StartAttackWaiting()
        {
            waitingToAttack = true;
        }

        public void StopAttackWaiting()
        {
            waitingToAttack = false;
            attackTimer = 0f;

            if (attackTimerImage != null)
                attackTimerImage.fillAmount = 0f;
        }
    }
}