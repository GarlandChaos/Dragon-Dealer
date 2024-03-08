using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class RunToIdleState : AState
    {
        private bool finishedRun = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            //Trigger run animation

            entityController.MovementController.MoveToInitialPosition(OnCompleteRunCallback);
        }

        public override IState Execute()
        {
            if (finishedRun)
                return new IdleState();

            return null;
        }

        public override void Exit()
        {
            entityController.CombatController.StartWaitingToAttack();
            CombatManager.Instance.FinishCurrentCombat();
        }

        private void OnCompleteRunCallback()
        {
            finishedRun = true;
        }
    }
}