using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay
{
    public class EntityController : MonoBehaviour
    {
        //HealthController
        //MovementController
        //CombatController
        [SerializeField] bool isPlayer = false;

        [SerializeField] private Image bodyImage = null;
        [SerializeField] private HealthController healthController = null;
        [SerializeField] private MovementController movementController = null;
        [SerializeField] private CombatController combatController = null;

        public Image BodyImage => bodyImage;
        public HealthController HealthController => healthController;
        public MovementController MovementController => movementController;
        public CombatController CombatController => combatController;

        // Start is called before the first frame update
        void Start()
        {
            if (!isPlayer)
            {
                string[] elementArray = Enum.GetNames(typeof(Element));
                int index = UnityEngine.Random.Range(1, elementArray.Length);
                if (!Enum.TryParse(elementArray[index], out Element element))
                    element = Element.GRASS;

                CombatController.SetEntityElement(element);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}