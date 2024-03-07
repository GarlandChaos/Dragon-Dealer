using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class AttackState : AState
    {
        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            //Trigger attack animation
        }

        public override IState Execute()
        {
            return null;
        }

        public override void Exit()
        {

        }
    }
}