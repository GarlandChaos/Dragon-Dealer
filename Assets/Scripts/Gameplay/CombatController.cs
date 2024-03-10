using Game.Gameplay.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.Gameplay
{
    public class CombatController : MonoBehaviour
    {
        private EntityController entityController = null;

        private bool initialized = false;
        private bool isChargingAttack = true;
        private float attackTimer = 0f;
        [SerializeField] private float waitForAttackDuration = 0f;

        public Action<float, float> onAttackTimerUpdated = null;

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

            isChargingAttack = true;
            attackTimer = 0f;

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
        }

        public void StopChargingAttack()
        {
            isChargingAttack = false;
            attackTimer = 0f;

            onAttackTimerUpdated?.Invoke(attackTimer, waitForAttackDuration);
        }
    }
}