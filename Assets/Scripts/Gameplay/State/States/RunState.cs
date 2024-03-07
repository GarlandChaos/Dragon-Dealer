using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace Game.Gameplay.State
{
    public class RunState : AState
    {
        private bool finishedRun = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            //Trigger run animation

            entityController.transform.DOMove(CombatManager.Instance.CurrentCombatPacket.target.transform.position, 2f)
                .OnComplete(() => 
                {
                    finishedRun = true;                
                });
        }

        public override IState Execute()
        {
            if (finishedRun)
                return new IdleState();

            return null;
        }

        public override void Exit()
        {

        }
    }
}