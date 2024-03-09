using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class DeckManager : ASingleton<DeckManager>
    {
        //Sort Card from list of possible cards (sort element and value)
        //Return Card

        [Header("Object Data")]
        [SerializeField] private int minCardValue = 5; //TODO: Make scriptable object DeckManagerSettings
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