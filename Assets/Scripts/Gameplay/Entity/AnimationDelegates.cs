using UnityEngine;

namespace Game.Gameplay
{
    public class AnimationDelegates : MonoBehaviour
    {
        [SerializeField] private EntityController entityController = null;

        public void OnFinishedAttack()
        {
            entityController.CombatController.FinishAttack();
        }
    }
}