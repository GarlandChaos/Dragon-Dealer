using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Combat
{
    public struct CombatPacket 
    {
        public EntityController attacker;
        public EntityController target;
        public Card card;

        public CombatPacket(EntityController attacker, EntityController target, Card card)
        {
            this.attacker = attacker;
            this.target = target;
            this.card = card;
        }
    }

    public class CombatManager : ASingleton<CombatManager>
    {
        public event Action<CombatPacket> onCombatPacketCreated = null;
        public event Action onCurrentCombatFinished = null;
        private Queue<CombatPacket> combatPacketQueue = new();

        //Properties
        public CombatPacket CurrentCombatPacket => combatPacketQueue.Peek();

        private void Start()
        {
            LevelManager.Instance.onLevelStarted += OnLevelStarted;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.onLevelStarted -= OnLevelStarted;
        }

        public void CreateCombatPacket(EntityController attacker, EntityController target, Card card)
        {
            CombatPacket packet = new CombatPacket(attacker, target, card);
            combatPacketQueue.Enqueue(packet);

            if (combatPacketQueue.Count > 1) return;
            
            onCombatPacketCreated?.Invoke(packet);
        }

        public void FinishCurrentCombat()
        {
            combatPacketQueue.Dequeue();

            onCurrentCombatFinished?.Invoke();

            if(combatPacketQueue.Count > 0)
                onCombatPacketCreated?.Invoke(combatPacketQueue.Peek());
        }

        private void OnLevelStarted()
        {
            combatPacketQueue.Clear();
        }
    }
}