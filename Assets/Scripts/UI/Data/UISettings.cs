using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "Game Data/UI Settings", fileName = "UI Settings")]
    public class UISettings : ScriptableObject
    {
        [Header("Screen prefabs")]
        [SerializeField] private List<GameObject> screens = new List<GameObject>();

        [Header("Other settings")]
        [SerializeField] private bool hideScreensUponInstantiation = false;

        //Properties
        public List<GameObject> Screens { get => screens; }
        public bool HideScreensUponInstantiation { get => hideScreensUponInstantiation; }
    }
}