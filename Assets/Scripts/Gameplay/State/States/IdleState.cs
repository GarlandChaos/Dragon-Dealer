using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class IdleState : AState
    {
        private bool attackStarted = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            CombatManager.Instance.onAttackStarted += OnAttackStarted;
            //Trigger idle animation
        }

        private void OnAttackStarted(CombatPacket combatPacket)
        {
            if (combatPacket.attacker != entityController) return;

            attackStarted = true;
        }

        public override IState Execute()
        {
            if(attackStarted)
                return new RunState();

            return null;
        }

        public override void Exit()
        {
            CombatManager.Instance.onAttackStarted -= OnAttackStarted;
        }
    }
}