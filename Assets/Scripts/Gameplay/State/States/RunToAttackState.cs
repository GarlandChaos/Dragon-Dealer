using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class RunToAttackState : AState
    {
        private bool finishedRun = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            //Trigger run animation

            Vector3 targetPosition = CombatManager.Instance.CurrentCombatPacket.target.transform.position;
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

        }

        private void OnCompleteRunCallback()
        {
            finishedRun = true;
        }
    }
}