using UnityEngine;
using DG.Tweening;
using Game.Gameplay;

namespace Game.UI
{
    public class DamageNumberElement : MonoBehaviour
    {
        [Header("Object Data")]
        private float yOffset = 1f;
        private float moveDuration = 0.8f;
        [SerializeField] private Color healColor = Color.green;
        [SerializeField] private Color normalDamageColor = Color.white;
        [SerializeField] private Color effectiveDamageColor = Color.yellow;

        [Header("Self Contained References")]
        [SerializeField] private TextSetter damageTextSetter = null;

        public void Initialize(int damage, Vector3 initialPosition, DamageType damageType = DamageType.NORMAL)
        {
            damageTextSetter.SetText(damage.ToString());
            SetDamageTextColor(damageType);

            transform.position = initialPosition;
            float yFinal = initialPosition.y + yOffset;
            transform.DOMoveY(yFinal, moveDuration).onComplete += () => DamageNumberElementManager.Instance.ReleaseDamageNumberElement(this);
        }

        private void SetDamageTextColor(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.NORMAL:
                    damageTextSetter.TMP_Text.color = normalDamageColor;
                    break;
                case DamageType.EFFECTIVE:
                    damageTextSetter.TMP_Text.color = effectiveDamageColor;
                    break;
                case DamageType.HEAL:
                    damageTextSetter.TMP_Text.color = healColor;
                    break;
                default:
                    break;
            }
        }
    }
}