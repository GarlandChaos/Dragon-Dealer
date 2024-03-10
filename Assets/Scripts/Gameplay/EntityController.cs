using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Gameplay.State;
using Game.UI;

namespace Game.Gameplay
{
    public class EntityController : MonoBehaviour
    {
        private Element element = Element.NONE;

        [SerializeField] private bool isPlayer = false;

        [SerializeField] private SpriteRenderer bodySpriteRenderer = null;
        [SerializeField] private StateMachine stateMachine = null;
        [SerializeField] private HealthController healthController = null;
        [SerializeField] private MovementController movementController = null;
        [SerializeField] private CombatController combatController = null;
        [SerializeField] private EntityAnimatorController animatorController = null;
        private EntityUIController entityUIController = null;

        public Element Element => element;
        public bool IsPlayer => isPlayer;
        public SpriteRenderer BodyImage => bodySpriteRenderer;
        public HealthController HealthController => healthController;
        public MovementController MovementController => movementController;
        public CombatController CombatController => combatController;
        public EntityAnimatorController AnimatorController => animatorController;
        public EntityUIController EntityUIController => entityUIController;

        private void Start()
        {
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;    
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }

        public void Initialize()
        {
            entityUIController = isPlayer ? 
                EntityUIControllerManager.Instance.GetPlayerUIController() : 
                EntityUIControllerManager.Instance.GetEntityUIController();
            entityUIController.Initialize(this);

            gameObject.name = isPlayer ? "Player Controller" : "Enemy Controller " + EntityControllerManager.Instance.ActiveEntityControllerCount.ToString();

            movementController.Reset();
            healthController.Initialize(this);
            combatController.Initialize(this);
            stateMachine.Initialize(this, new BaseState());
            
            if (!IsPlayer) return;

            SetEntityElement(Element.NONE);
        }

        public void SetEntityElement(Element element)
        {
            this.element = element;
            bodySpriteRenderer.color = GetElementColor(element);
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

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.NotInitialized:
                case GameState.MainMenu:
                    stateMachine.Initialize(this, new BaseState());
                    combatController.StopChargingAttack();
                    movementController.Reset();
                    break;
                case GameState.WaveStart:
                    stateMachine.Initialize(this, new BaseState());
                    combatController.StopChargingAttack();
                    movementController.Reset();
                    break;
                case GameState.GameRunning:
                    stateMachine.Initialize(this, new IdleState());
                    combatController.ChargeAttack();
                    break;
                case GameState.GamePause:
                    break;
                case GameState.GameEnd:
                    stateMachine.Initialize(this, new BaseState());
                    combatController.StopChargingAttack();
                    movementController.Reset();
                    break;
                default:
                    break;
            }
        }
    }
}