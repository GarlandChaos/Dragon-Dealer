using CartoonFX;
using Game.Audio;
using Game.Gameplay.Combat;
using Game.UI;

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
                EntityController target = CombatManager.Instance.CurrentCombatPacket.target;
                Card card = CombatManager.Instance.CurrentCombatPacket.card;
                
                int damage = entityController.CombatController.CalculateDamage(card);
                target.HealthController.TakeDamage(damage);
                
                CFXR_Effect particle = ParticleManager.Instance.GetHitParticle();
                particle.transform.position = target.HitParticleReferenceTransform.position;
                
                DamageNumberElement damageNumberElement = DamageNumberElementManager.Instance.GetDamageNumberElement();
                damageNumberElement.Initialize(damage, target.transform.position);

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