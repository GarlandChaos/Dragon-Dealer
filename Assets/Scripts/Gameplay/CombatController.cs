using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class CombatController : MonoBehaviour
    {
        [SerializeField] private EntityController entityController = null;
        private Element element = Element.NONE;

        public void SetEntityElement(Element element)
        {
            this.element = element;
            entityController.BodyImage.color = GetElementColor(element);
        }


        public Color GetElementColor(Element element)
        {
            return element switch
            {
                Element.GRASS => Color.green,
                Element.FIRE => Color.red,
                Element.WATER => Color.blue,
                _ => Color.white
            };
        }
    }
}