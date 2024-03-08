using Game.Gameplay.State;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay
{
    public class EntityController : MonoBehaviour
    {
        private Element element = Element.NONE;

        [SerializeField] private bool isPlayer = false;

        [SerializeField] private Image bodyImage = null;
        [SerializeField] private StateMachine stateMachine = null;
        [SerializeField] private HealthController healthController = null;
        [SerializeField] private MovementController movementController = null;
        [SerializeField] private CombatController combatController = null;

        public Element Element => element;
        public bool IsPlayer => isPlayer;
        public Image BodyImage => bodyImage;
        public HealthController HealthController => healthController;
        public MovementController MovementController => movementController;
        public CombatController CombatController => combatController;

        private void Start()
        {
            if (!isPlayer)
            {
                string[] elementArray = Enum.GetNames(typeof(Element));
                int index = UnityEngine.Random.Range(1, elementArray.Length);
                if (!Enum.TryParse(elementArray[index], out Element element))
                    element = Element.GRASS;

                SetEntityElement(element);
            }

            if (stateMachine == null)
                stateMachine = GetComponent<StateMachine>();

            stateMachine.Initialize(this, new IdleState());
        }

        public void SetEntityElement(Element element)
        {
            this.element = element;
            bodyImage.color = GetElementColor(element);
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