using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game.Gameplay;
using Game.Gameplay.Combat;

namespace Game.UI
{
    public class EntityUIController : MonoBehaviour
    {
        private EntityController entityController = null;
        private const string healthDisplayDivider = "/";
        private Vector3 initialBodyReferencePosition = Vector3.zero;

        [SerializeField] private RectTransform bodyPositionReferenceRectTransform = null;
        [SerializeField] private CardDropController cardDropController = null;
        [SerializeField] private TextSetter healthTextSetter = null;
        [SerializeField] private Image attackTimerImage = null;

        public Action<EntityUIController> onDeactivateEntityUIController = null;

        //Properties
        public EntityController EntityController => entityController;

        public void Initialize(EntityController entityController)
        {
            CombatManager.Instance.onCombatPacketCreated += OnCombatPacketCreated;
            CombatManager.Instance.onCurrentCombatFinished += OnCurrentCombatFinished;

            this.entityController = entityController;
            entityController.HealthController.onHealthUpdated += OnHealthUpdated;
            entityController.CombatController.onAttackTimerUpdated += OnAttackTimerUpdated;

            cardDropController.Initialize(entityController);

            initialBodyReferencePosition = bodyPositionReferenceRectTransform.position;
            SetEntityBodyPosition();

            bool isPlayer = entityController.IsPlayer;
            attackTimerImage.gameObject.SetActive(!isPlayer);
            cardDropController.gameObject.SetActive(true);
        }

        public void SetHealthTextSetter(TextSetter healthTextSetter, EnemyHealthElement enemyHealthElement = null)
        {
            this.healthTextSetter = healthTextSetter;
            entityController.HealthController.UpdateHealth();

            if (enemyHealthElement == null) return;

            entityController.HealthController.onEntityDead += (controller) => enemyHealthElement.DisableGraphics();
        }

        public void OnHealthUpdated(int currentHealthPoints, int healthPointsMax)
        {
            if (healthTextSetter == null) return;

            string healthString = currentHealthPoints.ToString() + healthDisplayDivider + healthPointsMax;
            healthTextSetter.SetText(healthString);
        }

        private void OnCombatPacketCreated(CombatPacket packet)
        {
            if (packet.attacker == packet.target) return;

            cardDropController.gameObject.SetActive(false);
        }

        private void OnCurrentCombatFinished()
        {
            cardDropController.gameObject.SetActive(true);
        }

        private void OnAttackTimerUpdated(float attackTimer, float waitForAttackDuration)
        {
            if(attackTimer == 0f)
            {
                attackTimerImage.fillAmount = 0f;
                return;
            }

            attackTimerImage.fillAmount = attackTimer / waitForAttackDuration;
        }

        public void SetEntityBodyPosition()
        {
            StartCoroutine(SetEntityBodyPositionRoutine());
        }

        private IEnumerator SetEntityBodyPositionRoutine()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            yield return wait;

            Vector3 bodyPosition = bodyPositionReferenceRectTransform.position;
            while (bodyPosition == initialBodyReferencePosition)
            {
                yield return wait;
                bodyPosition = bodyPositionReferenceRectTransform.position;
            }
            entityController.transform.position = bodyPosition;
            entityController.MovementController.SetInitialPosition(bodyPosition);
        }

        public void Deactivate()
        {
            onDeactivateEntityUIController?.Invoke(this);

            CombatManager.Instance.onCombatPacketCreated -= OnCombatPacketCreated;
            CombatManager.Instance.onCurrentCombatFinished -= OnCurrentCombatFinished;

            if (entityController == null) return;

            entityController.HealthController.onHealthUpdated -= OnHealthUpdated;
            entityController.CombatController.onAttackTimerUpdated -= OnAttackTimerUpdated;

            Delegate[] delegateList = onDeactivateEntityUIController?.GetInvocationList();
            if (delegateList == null) return;

            foreach (Delegate dlgt in delegateList)
                onDeactivateEntityUIController -= (Action<EntityUIController>)dlgt;
        }
    }
}