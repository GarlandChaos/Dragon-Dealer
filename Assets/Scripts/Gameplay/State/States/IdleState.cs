using UnityEngine;
using Game.Gameplay.Combat;
using Game.UI;
using Game.Audio;

namespace Game.Gameplay.State
{
    public class IdleState : BaseState
    {
        private bool attackStarted = false;
        private bool cardUsedOnItself = false;
        private bool hasNoHealthPoints = false;

        public override void Enter(EntityController entityController)
        {
            base.Enter(entityController);
            CombatManager.Instance.onCombatPacketCreated += OnCombatPacketCreated;
            entityController.HealthController.onEntityDead += OnEntityDead;
            entityController.AnimatorController.EnableIdleState();
        }

        public override IState Execute()
        {
            if (hasNoHealthPoints)
                return new DeadState();

            if (cardUsedOnItself)
            {
                cardUsedOnItself = false;

                Element element = CombatManager.Instance.CurrentCombatPacket.card.element;
                if (entityController.Element != element)
                    entityController.SetEntityElement(element);
                else
                {
                    int healthPointsToAdd = CombatManager.Instance.CurrentCombatPacket.card.value;
                    entityController.HealthController.AddHealth(healthPointsToAdd);

                    Vector3 targetPosition = CombatManager.Instance.CurrentCombatPacket.target.transform.position;
                    DamageNumberElement damageNumberElement = DamageNumberElementManager.Instance.GetDamageNumberElement();
                    damageNumberElement.Initialize(healthPointsToAdd, targetPosition, DamageType.HEAL);
                    
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.HealAudioClip);
                }

                CombatManager.Instance.FinishCurrentCombat();
                return null;
            }

            if(attackStarted)
                return new RunToAttackState();

            return null;
        }

        public override void Exit()
        {
            entityController.CombatController.StopChargingAttack();
            CombatManager.Instance.onCombatPacketCreated -= OnCombatPacketCreated;
            entityController.HealthController.onEntityDead -= OnEntityDead;
            entityController.AnimatorController.DisableIdleState();
        }

        private void OnCombatPacketCreated(CombatPacket combatPacket)
        {
            if (combatPacket.attacker != entityController) return;

            if (combatPacket.attacker == combatPacket.target)
            {
                cardUsedOnItself = true;
                return;
            }

            attackStarted = true;
        }

        public void OnEntityDead(EntityController deadEntityController)
        {
            if(deadEntityController != entityController) return;

            hasNoHealthPoints = true;
        }
    }
}