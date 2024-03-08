using DG.Tweening;
using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class AttackState : AState
    {
        private float attackTimer = 0f;
        private float attackDuration = 0.5f;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            //Trigger attack animation
        }

        public override IState Execute()
        {
            attackTimer += Time.deltaTime;

            if(attackTimer >= attackDuration)
            {
                Card card = CombatManager.Instance.CurrentCombatPacket.card;
                CombatManager.Instance.CurrentCombatPacket.target.HealthController.TakeDamage(card.value);
                return new RunToIdleState();
            }

            return null;
        }

        public override void Exit()
        {

        }
    }
}