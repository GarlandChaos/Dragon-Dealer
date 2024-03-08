using Game.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private EntityController entityController = null;
        [SerializeField] private Image dropAreaImage = null;
        [SerializeField] private Color defaultDropAreaColor = Color.black;

        public EntityController Entity => entityController;

        public void DropCard(EntityController attackerController, Card card)
        {
            CombatManager.Instance.CreateCombatPacket(attackerController, entityController, card);
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