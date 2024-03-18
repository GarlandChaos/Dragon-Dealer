using UnityEngine;
using Game.Gameplay.State;
using Game.UI;
using Game.Data;

namespace Game.Gameplay
{
    public class EntityController : MonoBehaviour
    {
        [Header("Object Data")]
        [SerializeField] private bool isPlayer = false;
        private Element element = Element.NONE;
        private EntityUIController entityUIController = null;
        private const string playerName = "Player Controller";
        private const string enemyName = "Enemy Controller ";

        [Header("Self Contained References")]
        [SerializeField] private Transform targetReferenceTransform = null;
        [SerializeField] private Transform hitParticleReferenceTransform = null;
        [SerializeField] private StateMachine stateMachine = null;
        [SerializeField] private SpriteRenderer bodySpriteRenderer = null;
        [SerializeField] private HealthController healthController = null;
        [SerializeField] private MovementController movementController = null;
        [SerializeField] private CombatController combatController = null;
        [SerializeField] private EntityAnimatorController animatorController = null;

        [Header("External Data")]
        [SerializeField] private ElementVisualData grassVisualData = null;
        [SerializeField] private ElementVisualData fireVisualData = null;
        [SerializeField] private ElementVisualData waterVisualData = null;

        //Properties
        public bool IsPlayer => isPlayer;
        public Element Element => element;
        public Transform TargetReferenceTransform => targetReferenceTransform;
        public Transform HitParticleReferenceTransform => hitParticleReferenceTransform;
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
            bodySpriteRenderer.enabled = false;

            entityUIController = isPlayer ? 
                EntityUIControllerManager.Instance.GetPlayerUIController() : 
                EntityUIControllerManager.Instance.GetEntityUIController();
            entityUIController.Initialize(this);

            gameObject.name = isPlayer ? playerName : enemyName + EntityControllerManager.Instance.ActiveEntityControllerCount.ToString();

            movementController.Reset();
            healthController.Initialize(this);
            combatController.Initialize(this);
            stateMachine.Initialize(this, new BaseState());
            
            if (!IsPlayer) return;

            SetEntityElement(Element.FIRE);
        }

        public void SetEntityElement(Element element)
        {
            this.element = element;

            switch (element)
            {
                case Element.GRASS:
                    SetAnimatorController(grassVisualData);
                    break;
                case Element.FIRE:
                    SetAnimatorController(fireVisualData);
                    break;
                case Element.WATER:
                    SetAnimatorController(waterVisualData);
                    break;
                case Element.NONE:
                default:
                    break;
            }
        }

        private void SetAnimatorController(ElementVisualData visualData)
        {
            EntityAnimationClips animationClips = isPlayer ? visualData.PlayerAnimationClips : visualData.EnemyAnimationClips;

            animatorController.SetAnimatorController(animationClips);
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
                    bodySpriteRenderer.enabled = false;
                    break;
                case GameState.GameRunning:
                    stateMachine.Initialize(this, new IdleState());
                    combatController.ChargeAttack();
                    bodySpriteRenderer.enabled = true;
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