using System.Collections.Generic;
using UnityEngine;
using Game.Utility.Pool;
using Game.UI;

namespace Game.Gameplay
{
    public class DamageNumberElementManager : ASingleton<DamageNumberElementManager>
    {
        [SerializeField] private DamageNumberElementPool damageNumberElementPool = null;
        private List<DamageNumberElement> activeDamageNumberElementList = new();

        //Properties
        public int ActiveEntityControllerCount => activeDamageNumberElementList.Count;

        public DamageNumberElement GetDamageNumberElement()
        {
            DamageNumberElement damageNumberElement = damageNumberElementPool.Pool.Get();
            activeDamageNumberElementList.Add(damageNumberElement);

            return damageNumberElement;
        }

        public void ReleaseDamageNumberElement(DamageNumberElement damageNumberElement)
        {
            activeDamageNumberElementList.Remove(damageNumberElement);
            damageNumberElementPool.Pool.Release(damageNumberElement);
        }

        public void ClearDamageNumberElementList()
        {
            foreach (DamageNumberElement damageNumberElement in activeDamageNumberElementList)
                damageNumberElementPool.Pool.Release(damageNumberElement);

            activeDamageNumberElementList.Clear();
        }
    }
}