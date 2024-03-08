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
        [SerializeField] private EntityController entityController = null;

        private bool isWaitingToAttack = true;
        private float attackTimer = 0;
        [SerializeField] private float waitForAttackDuration = 0;

        public Action<float, float> onAttackTimerUpdated = null;

        private void Update()
        {
            if (!entityController.IsPlayer) return;

            if (!isWaitingToAttack) return;

            attackTimer += Time.deltaTime;
            onAttackTimerUpdated?.Invoke(attackTimer, waitForAttackDuration);
            if(attackTimer >= waitForAttackDuration)
            {
                StopWaitingToAttack();
                Card attackCard = new Card(entityController.Element, UnityEngine.Random.Range(5, 26));
                CombatManager.Instance.CreateCombatPacket(entityController, GameManager.Instance.PlayerController, attackCard);
            }
        }

        public void StartWaitingToAttack()
        {
            isWaitingToAttack = true;
        }

        public void StopWaitingToAttack()
        {
            isWaitingToAttack = false;
            attackTimer = 0f;

            onAttackTimerUpdated?.Invoke(attackTimer, waitForAttackDuration);
        }
    }
}