using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class RunToIdleState : BaseState
    {
        private bool finishedRun = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            entityController.AnimatorController.EnableRunState();

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
            entityController.AnimatorController.DisableRunState();
            entityController.CombatController.ChargeAttack();
            CombatManager.Instance.FinishCurrentCombat();
        }

        private void OnCompleteRunCallback()
        {
            finishedRun = true;
        }
    }
}