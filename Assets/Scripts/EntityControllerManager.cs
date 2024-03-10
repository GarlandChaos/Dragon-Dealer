using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility.Pool;
using Game.Gameplay;

namespace Game
{
    public class EntityControllerManager : ASingleton<EntityControllerManager>
    {
        [SerializeField] private EntityControllerPool entityControllerPool = null;
        private List<EntityController> activeEntityControllerList = new();

        //Properties
        public int ActiveEntityControllerCount => activeEntityControllerList.Count;

        public EntityController GetEntityController()
        {
            EntityController controller = entityControllerPool.Pool.Get();
            activeEntityControllerList.Add(controller);

            return controller;
        }

        public void ReleaseEntityController(EntityController controller)
        {
            activeEntityControllerList.Remove(controller);
            entityControllerPool.Pool.Release(controller);
        }

        public void ClearEntityControllerList()
        {
            foreach (EntityController controller in activeEntityControllerList)
                entityControllerPool.Pool.Release(controller);

            activeEntityControllerList.Clear();
        }
    }
}