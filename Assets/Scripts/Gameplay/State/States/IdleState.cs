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

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            CombatManager.Instance.onCombatPacketCreated += OnCombatPacketCreated;
            //Trigger idle animation
        }

        private void OnCombatPacketCreated(CombatPacket combatPacket)
        {
            if (combatPacket.attacker != entityController) return;

            if(combatPacket.attacker == combatPacket.target)
            {
                cardUsedOnItself = true;
                return;
            }

            attackStarted = true;
        }

        public override IState Execute()
        {
            if (cardUsedOnItself)
            {
                Element element = CombatManager.Instance.CurrentCombatPacket.card.element;
                entityController.SetEntityElement(element);
                return null;
            }

            if(attackStarted)
                return new RunToAttackState();

            return null;
        }

        public override void Exit()
        {
            CombatManager.Instance.onCombatPacketCreated -= OnCombatPacketCreated;
        }
    }
}