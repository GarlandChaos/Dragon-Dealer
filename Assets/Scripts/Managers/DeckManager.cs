using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class DeckManager : ASingleton<DeckManager>
    {
        [Header("Object Data")]
        [SerializeField] private int minCardValue = 5;
        [SerializeField] private int maxCardValue = 15;

        public Card GetCard()
        {
            string[] elementArray = Enum.GetNames(typeof(Element));
            int index = UnityEngine.Random.Range(1, elementArray.Length);
            if (!Enum.TryParse(elementArray[index], out Element cardElement))
                cardElement = Element.GRASS;

            int cardValue = UnityEngine.Random.Range(minCardValue, maxCardValue);

            return new Card(cardElement, cardValue);
        }
    }
}