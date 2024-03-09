using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class DeadState : AState
    {
        private float destroyTimer = 0f;
        private float destroyTimerDuration = 0f;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            //Trigger dead animation
        }

        public override IState Execute()
        {
            destroyTimer += Time.deltaTime;

            if(destroyTimer >= destroyTimerDuration)
            {
                if (!entityController.IsPlayer)
                {
                    LevelManager.Instance.ReleaseDeadEnemy(entityController);
                }
                return new IdleState();
            }
            return null;
        }

        public override void Exit()
        {

        }
    }
}