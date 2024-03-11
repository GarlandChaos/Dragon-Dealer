using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Utility.Pool;

namespace Game
{
    public class EntityUIControllerManager : ASingleton<EntityUIControllerManager>
    {
        [SerializeField] private EntityUIController playerUIControllerPrefab = null;
        private EntityUIController playerUIController = null;
        [SerializeField] private EntityUIControllerPool entityUIControllerPool = null;
        private List<EntityUIController> activeEntityUIControllerList = new();

        //Properties
        public List<EntityUIController> ActiveEntityUIControllerList => activeEntityUIControllerList;
        public int ActiveEntityUIControllerCount => activeEntityUIControllerList.Count;

        public EntityUIController GetPlayerUIController()
        {
            if (playerUIController == null)
                playerUIController = Instantiate(playerUIControllerPrefab);

            return playerUIController;
        }

        public EntityUIController GetEntityUIController()
        {
            EntityUIController controller = entityUIControllerPool.Pool.Get();
            activeEntityUIControllerList.Add(controller);

            return controller;
        }

        public void ReleaseEntityUIController(EntityUIController controller)
        {
            activeEntityUIControllerList.Remove(controller);
            entityUIControllerPool.Pool.Release(controller);
        }

        public void ClearEntityUIControllerList()
        {
            foreach (EntityUIController controller in activeEntityUIControllerList)
                entityUIControllerPool.Pool.Release(controller);

            activeEntityUIControllerList.Clear();
        }
    }
}