using System;
using System.Collections.Generic;

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
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
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

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                    break;
                case GameState.MainMenu:
                    break;
                case GameState.WaveStart:
                    combatPacketQueue.Clear();
                    break;
                case GameState.GameRunning:
                    break;
                case GameState.GamePause:
                    break;
                case GameState.GameEnd:
                    combatPacketQueue.Clear();
                    break;
                default:
                    break;
            }
        }
    }
}