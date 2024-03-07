using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Game.Gameplay
{
    public class HealthController : MonoBehaviour
    {
        [Header("Object Data")]
        [SerializeField] private EntityController entityController = null;
        [SerializeField] private int healthPointsMax = 5;
        private int currentHealthPoints = 5;
        private const string healthDisplayDivider = "/";

        [Header("Self Contained References")]
        [SerializeField] private TextSetter healthTextSetter = null;

        //[Header("Game Events")]
        //[SerializeField] private GameEvent playerHealedEvent = null;
        //[SerializeField] private GameEvent entityDeadEvent = null;

        public int HealthPointsMax => healthPointsMax;
        public int CurrentHealthPoints => currentHealthPoints;
        public bool HasHealthPointsRemaining => currentHealthPoints > 0;

        private void Awake()
        {
            currentHealthPoints = healthPointsMax;
            SetHealthText();
        }

        public void TakeDamage(int damagePoints)
        {
            currentHealthPoints -= damagePoints;
            currentHealthPoints = Mathf.Clamp(currentHealthPoints, 0, healthPointsMax);
            SetHealthText();
            //if (currentHealthPoints <= 0)
            //    entityDeadEvent.Raise(entityController);
            //Debug.Log("Took damage, remaining health points: " + currentHealthPoints);
        }

        public void AddHealth(int healthPointsToAdd)
        {
            currentHealthPoints += healthPointsToAdd;
            currentHealthPoints = Mathf.Clamp(currentHealthPoints, 0, healthPointsMax);
            SetHealthText();
            //if (playerHealedEvent == null) return;

            //playerHealedEvent.Raise();
        }

        public void SetHealthText()
        {
            string healthString = currentHealthPoints.ToString() + healthDisplayDivider + healthPointsMax;
            healthTextSetter.SetText(healthString);
        }
    }
}