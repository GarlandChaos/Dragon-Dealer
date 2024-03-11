using Game.Gameplay.Combat;
using UnityEngine;
using UnityEngine.UI;
using Game.Audio;

namespace Game.Gameplay
{
    public interface ICardDropHandler 
    {
        public EntityController Entity { get; }
        public void DropCard(EntityController attackerController, Card card);
        public void SetDropImageColor(Color color);
        public void ResetDropHandler();
    }

    public class CardDropController : MonoBehaviour, ICardDropHandler
    {
        private EntityController entityController = null;
        [SerializeField] private Image dropAreaImage = null;
        [SerializeField] private Color defaultDropAreaColor = Color.black;

        public EntityController Entity => entityController;

        public void Initialize(EntityController entityController)
        {
            this.entityController = entityController;
        }

        public void DropCard(EntityController attackerController, Card card)
        {
            CombatManager.Instance.CreateCombatPacket(attackerController, entityController, card);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.DropCardAudioClip);
        }

        public void SetDropImageColor(Color color)
        {
            dropAreaImage.color = color;
        }

        public void ResetDropHandler()
        {
            dropAreaImage.color = defaultDropAreaColor;
        }
    }
}