using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public struct WaveUnit
    {
        public Element element;
        public int numberOfEnemies;
    }

    [CreateAssetMenu(menuName = "Game Data/Wave", fileName = "Wave")]
    public class Wave : ScriptableObject
    {
        [SerializeField] private List<WaveUnit> waveUnitList = new();

        //Properties
        public List<WaveUnit> WaveUnitList => waveUnitList;

        public int GetTotalUnitCount()
        {
            int totalUnitCount = 0;
            foreach(WaveUnit waveUnit in waveUnitList)
            {
                totalUnitCount += waveUnit.numberOfEnemies;
            }

            return totalUnitCount;
        }
    }
}