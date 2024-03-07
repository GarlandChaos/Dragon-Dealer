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
        public event Action<CombatPacket> onAttackStarted = null;
        private Stack<CombatPacket> combatPacketStack = new();

        //Properties
        public CombatPacket CurrentCombatPacket => combatPacketStack.Peek();

        public void CreateCombatPacket(EntityController attacker, EntityController target, Card card)
        {
            CombatPacket packet = new CombatPacket(attacker, target, card);
            combatPacketStack.Push(packet);
            onAttackStarted?.Invoke(packet);
        }
    }
}