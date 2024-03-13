using CartoonFX;
using DG.Tweening;
using Game.Audio;
using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.State
{
    public class AttackState : BaseState
    {
        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            entityController.AnimatorController.EnableAttackState();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.HitAudioClip);
        }

        public override IState Execute()
        {
            if(entityController.CombatController.HasFinishedAttack)
            {
                Card card = CombatManager.Instance.CurrentCombatPacket.card;
                int damage = entityController.CombatController.CalculateDamage(card);
                CombatManager.Instance.CurrentCombatPacket.target.HealthController.TakeDamage(damage);
                CFXR_Effect particle = ParticleManager.Instance.GetHitParticle();
                particle.transform.position = CombatManager.Instance.CurrentCombatPacket.target.HitParticleReferenceTransform.position;

                return new RunToIdleState();
            }

            return null;
        }

        public override void Exit()
        {
            entityController.AnimatorController.DisableAttackState();
        }
    }
}