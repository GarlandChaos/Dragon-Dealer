using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class IdleState : AState
    {
        private bool attackStarted = false;
        private bool cardUsedOnItself = false;
        private bool hasNoHealthPoints = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            CombatManager.Instance.onCombatPacketCreated += OnCombatPacketCreated;
            entityController.HealthController.onEntityDead += OnEntityDead;
            //Trigger idle animation
        }

        public override IState Execute()
        {
            if (hasNoHealthPoints)
                return new DeadState();

            if (cardUsedOnItself)
            {
                cardUsedOnItself = false;
                Element element = CombatManager.Instance.CurrentCombatPacket.card.element;
                entityController.SetEntityElement(element);
                CombatManager.Instance.FinishCurrentCombat();
                return null;
            }

            if(attackStarted)
                return new RunToAttackState();

            return null;
        }

        public override void Exit()
        {
            entityController.CombatController.StopChargingAttack();
            CombatManager.Instance.onCombatPacketCreated -= OnCombatPacketCreated;
            entityController.HealthController.onEntityDead -= OnEntityDead;
        }

        private void OnCombatPacketCreated(CombatPacket combatPacket)
        {
            if (combatPacket.attacker != entityController) return;

            if (combatPacket.attacker == combatPacket.target)
            {
                cardUsedOnItself = true;
                return;
            }

            attackStarted = true;
        }

        public void OnEntityDead(EntityController deadEntityController)
        {
            if(deadEntityController != entityController) return;

            hasNoHealthPoints = true;
        }
    }
}