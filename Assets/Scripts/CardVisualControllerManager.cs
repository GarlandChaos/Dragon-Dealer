using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility.Pool;

namespace Game.Gameplay
{
    public class CardVisualControllerManager : ASingleton<CardVisualControllerManager>
    {
        [SerializeField] private CardVisualControllerPool cardVisualControllerPool = null;
        private List<CardVisualController> activeCardVisualControllerList = new();

        public CardVisualController GetCardVisualController()
        {
            CardVisualController controller = cardVisualControllerPool.Pool.Get();
            activeCardVisualControllerList.Add(controller);

            return controller;
        }

        public void ReleaseCardVisualController(CardVisualController controller)
        {
            activeCardVisualControllerList.Remove(controller);
            cardVisualControllerPool.Pool.Release(controller);
        }

        public void ClearCardVisualControllerList()
        {
            foreach (CardVisualController controller in activeCardVisualControllerList)
                cardVisualControllerPool.Pool.Release(controller);

            activeCardVisualControllerList.Clear();
        }
    }
}