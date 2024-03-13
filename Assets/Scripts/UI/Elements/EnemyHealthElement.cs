using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EnemyHealthElement : MonoBehaviour
    {
        //Object data
        private const string zeroHealthString = "0/";

        [Header("Self Contained References")]
        [SerializeField] private Image healthIconImage = null;
        [SerializeField] private TextSetter healthTextSetter = null;

        public TextSetter HealthTextSetter => healthTextSetter;

        private void Start()
        {
            //TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        private void OnTextChanged(Object obj)
        {
            if (obj == healthTextSetter.TMP_Text)
            {
                if (!healthTextSetter.Text.Contains(zeroHealthString)) return;
                if (CanvasUpdateRegistry.IsRebuildingGraphics() || CanvasUpdateRegistry.IsRebuildingLayout()) return;
                DisableGraphics();
            }
        }

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