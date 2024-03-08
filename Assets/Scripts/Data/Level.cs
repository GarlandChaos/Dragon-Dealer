using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Game Data/Level", fileName = "Level")]
    public class Level : ScriptableObject
    {
        [SerializeField] private List<Wave> waveList = new();

        //Properties
        public List<Wave> WaveList => waveList;
    }
}