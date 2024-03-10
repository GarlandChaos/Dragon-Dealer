using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EnemyHealthElement : MonoBehaviour
    {
        [SerializeField] private Image healthIconImage = null;
        [SerializeField] private TextSetter healthTextSetter = null;

        public TextSetter HealthTextSetter => healthTextSetter;

        public void SetHealthIconSprite(Sprite healthIconSprite)
        {
            healthIconImage.sprite = healthIconSprite;
        }
    }
}