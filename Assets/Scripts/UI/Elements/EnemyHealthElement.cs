using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EnemyHealthElement : MonoBehaviour
    {
        [Header("Self Contained References")]
        [SerializeField] private Image healthIconImage = null;
        [SerializeField] private TextSetter healthTextSetter = null;

        //Properties
        public TextSetter HealthTextSetter => healthTextSetter;

        public void EnableGraphics()
        {
            HealthTextSetter.TMP_Text.enabled = true;
            healthIconImage.enabled = true;
        }

        public void DisableGraphics()
        {
            HealthTextSetter.TMP_Text.enabled = false;
            healthIconImage.enabled = false;
        }

        public void SetHealthIconSprite(Sprite healthIconSprite)
        {
            healthIconImage.sprite = healthIconSprite;
        }
    }
}