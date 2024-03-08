using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private CardDropController cardDropController = null;
        [SerializeField] private TextSetter healthTextSetter = null;
        [SerializeField] private Image attackTimerImage = null;

        private void Start()
        {
            CombatManager.Instance.onCombatPacketCreated += OnCombatPacketCreated;
            CombatManager.Instance.onCurrentCombatFinished += OnCurrentCombatFinished;
        }

        private void OnDestroy()
        {
            CombatManager.Instance.onCombatPacketCreated -= OnCombatPacketCreated;
            CombatManager.Instance.onCurrentCombatFinished -= OnCurrentCombatFinished;

            if (entityController == null) return;

            entityController.HealthController.onHealthUpdated -= SetHealthText;
            entityController.CombatController.onAttackTimerUpdated -= OnAttackTimerUpdated;
        }

        public void Initialize(EntityController entityController)
        {
            this.entityController = entityController;
            entityController.HealthController.onHealthUpdated += SetHealthText;
            entityController.CombatController.onAttackTimerUpdated += OnAttackTimerUpdated;
            
            bool isPlayer = entityController.IsPlayer;
            attackTimerImage.gameObject.SetActive(!isPlayer);
        }

        public void SetHealthText(int currentHealthPoints, int healthPointsMax)
        {
            string healthString = currentHealthPoints.ToString() + healthDisplayDivider + healthPointsMax;
            healthTextSetter.SetText(healthString);
        }

        private void OnCombatPacketCreated(CombatPacket obj)
        {
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
    }
}