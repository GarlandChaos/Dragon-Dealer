using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class RunToAttackState : BaseState
    {
        private bool finishedRun = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            entityController.AnimatorController.EnableRunState();

            Vector3 targetPosition = CombatManager.Instance.CurrentCombatPacket.target.TargetReferenceTransform.position;
            entityController.MovementController.Move(targetPosition, OnCompleteRunCallback);
        }

        public override IState Execute()
        {
            if (finishedRun)
                return new AttackState();

            return null;
        }

        public override void Exit()
        {
            entityController.AnimatorController.DisableRunState();
        }

        private void OnCompleteRunCallback()
        {
            finishedRun = true;
        }
    }
}