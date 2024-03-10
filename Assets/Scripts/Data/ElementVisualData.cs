using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Gameplay;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game Data/Element Visual Data", fileName = "Element Visual Data")]
    public class ElementVisualData : ScriptableObject
    {
        [SerializeField] private Element element = Element.NONE;
        [SerializeField] private Sprite cardSprite = null;
        [SerializeField] private Sprite chargeAttackTimerSprite = null;
        [SerializeField] private Sprite healthIconSprite = null;
        [SerializeField] private Color color = Color.white;

        //Properties
        public Element Element => element;
        public Sprite CardSprite => cardSprite;
        public Sprite ChargeAttackTimerSprite => chargeAttackTimerSprite;
        public Sprite HealthIconSprite => healthIconSprite;
        public Color Color => color;
    }
}