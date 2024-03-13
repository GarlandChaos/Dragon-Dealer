using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using System;
using CartoonFX;

namespace Game.Gameplay
{
    public class HealthController : MonoBehaviour
    {
        [Header("Object Data")]
        private EntityController entityController = null;
        [SerializeField] private int healthPointsMax = 5;
        private int currentHealthPoints = 5;

        public Action<int, int> onHealthUpdated = null;
        public Action<EntityController> onEntityDead = null;

        public int HealthPointsMax => healthPointsMax;
        public int CurrentHealthPoints => currentHealthPoints;
        public bool HasHealthPointsRemaining => currentHealthPoints > 0;

        private void OnDisable()
        {
            Delegate[] delegateList = onEntityDead?.GetInvocationList();
            if (delegateList == null) return;

            foreach(Delegate dlgt in delegateList)
                onEntityDead -= (Action<EntityController>) dlgt;
        }

        public void Initialize(EntityController entityController)
        {
            this.entityController = entityController;
            currentHealthPoints = healthPointsMax;
            onHealthUpdated?.Invoke(currentHealthPoints, healthPointsMax);
        }

        public void TakeDamage(int damagePoints)
        {
            currentHealthPoints -= damagePoints;
            currentHealthPoints = Mathf.Clamp(currentHealthPoints, 0, healthPointsMax);
            UpdateHealth();

            if (currentHealthPoints <= 0)
            {
                CFXR_Effect particle = ParticleManager.Instance.GetDeadParticle();
                particle.gameObject.transform.position = entityController.transform.position;
                onEntityDead?.Invoke(entityController);
            }
        }

        public void AddHealth(int healthPointsToAdd)
        {
            currentHealthPoints += healthPointsToAdd;
            currentHealthPoints = Mathf.Clamp(currentHealthPoints, 0, healthPointsMax);
            UpdateHealth();
        }

        public void UpdateHealth()
        {
            onHealthUpdated?.Invoke(currentHealthPoints, healthPointsMax);
        }
    }
}