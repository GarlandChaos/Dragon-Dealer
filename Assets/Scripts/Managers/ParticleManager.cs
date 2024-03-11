using CartoonFX;
using Game.Utility.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class ParticleManager : ASingleton<ParticleManager>
    {
        [SerializeField] ParticleEffectPool hitParticleEffectPool = null;
        [SerializeField] ParticleEffectPool deadParticleEffectPool = null;
        
        private List<GameObject> deadParticleList = new();
        private List<GameObject> hitParticleList = new();

        public CFXR_Effect GetHitParticle()
        {
            CFXR_Effect particle = hitParticleEffectPool.Pool.Get();
            hitParticleList.Add(particle.gameObject);
            particle.ResetState();

            return particle;
        }

        public CFXR_Effect GetDeadParticle()
        {
            CFXR_Effect particle = deadParticleEffectPool.Pool.Get();
            deadParticleList.Add(particle.gameObject);
            particle.ResetState();

            return particle;
        }
    }
}