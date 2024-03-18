using UnityEngine;
using DG.Tweening;
using Game.Gameplay;

namespace Game.UI
{
    public class DamageNumberElement : MonoBehaviour
    {
        //Object data
        private float yOffset = 2f;
        private float moveDuration = 2f;

        [Header("Self Contained References")]
        [SerializeField] private TextSetter damageTextSetter = null;

        public void Initialize(int damage, Vector3 initialPosition)
        {
            damageTextSetter.SetText(damage.ToString());

            transform.position = initialPosition;
            float yFinal = initialPosition.y + yOffset;
            transform.DOMoveY(yFinal, moveDuration).onComplete += () => DamageNumberElementManager.Instance.ReleaseDamageNumberElement(this);
        }
    }
}