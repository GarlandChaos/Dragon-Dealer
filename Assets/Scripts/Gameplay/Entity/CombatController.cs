using System;
using UnityEngine;
using Game.Gameplay.Combat;

namespace Game.Gameplay
{
    public enum DamageType
    {
        NORMAL,
        EFFECTIVE,
        HEAL
    }

    public class CombatController : MonoBehaviour
    {
        [Header("Object Data")]
        private EntityController entityController = null;
        private bool initialized = false;
        private bool isChargingAttack = true;
        private bool hasFinishedAttack = false;
        private float attackTimer = 0f;
        [SerializeField] private float waitForAttackDuration = 0f;

        //Events
        public Action<float, float> onAttackTimerUpdated = null;

        //Properties
        public bool HasFinishedAttack => hasFinishedAttack;

        private void Start()
        {
            CombatManager.Instance.onCombatPacketCreated += OnCombatPacketCreated;
            CombatManager.Instance.onCurrentCombatFinished += OnCurrentCombatFinished;
        }

        private void Update()
        {
            if (!initialized) return;

            if (entityController.IsPlayer) return;

            if (!isChargingAttack) return;

            attackTimer += Time.deltaTime;

            onAttackTimerUpdated?.Invoke(attackTimer, waitForAttackDuration);
            if (attackTimer >= waitForAttackDuration)
            {
                StopChargingAttack();
                Card attackCard = new Card(entityController.Element, UnityEngine.Random.Range(5, 26));
                CombatManager.Instance.CreateCombatPacket(entityController, LevelManager.Instance.PlayerController, attackCard);
            }
        }

        public void Initialize(EntityController entityController)
        {
            this.entityController = entityController;

            isChargingAttack = false;
            attackTimer = 0f;
            hasFinishedAttack = false;

            initialized = true;
        }

        private void OnCurrentCombatFinished()
        {
            ChargeAttack();
        }

        private void OnCombatPacketCreated(CombatPacket packet)
        {
            isChargingAttack = false;
        }

        public void ChargeAttack()
        {
            isChargingAttack = true;
            hasFinishedAttack = false;
        }

        public void StopChargingAttack()
        {
            isChargingAttack = false;
            attackTimer = 0f;
            hasFinishedAttack = false;

            onAttackTimerUpdated?.Invoke(attackTimer, waitForAttackDuration);
        }

        public void FinishAttack()
        {
            hasFinishedAttack = true;
        }

        public int CalculateDamage(Card card)
        {
            Element elementWeakness = GetElementWeakness(entityController.Element);
            if (elementWeakness == card.element)
                return card.value * 2;
            
            return card.value;
        }

        public Element GetElementWeakness(Element element)
        {
            return element switch
            {
                Element.GRASS => Element.FIRE,
                Element.FIRE => Element.WATER,
                Element.WATER => Element.GRASS,
                Element.NONE => Element.NONE,
                _ => Element.NONE,
            };
        }

        public DamageType GetDamageType(Element element)
        {
            Element elementWeakness = GetElementWeakness(entityController.Element);
            if (element == elementWeakness)
                return DamageType.EFFECTIVE;

            if (element == entityController.Element)
                return DamageType.HEAL;

            return DamageType.NORMAL;
        }
    }
}