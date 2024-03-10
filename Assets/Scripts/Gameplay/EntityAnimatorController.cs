using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class EntityAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        private int idleHash = Animator.StringToHash("Idle");
        private int runHash = Animator.StringToHash("Run");
        private int attackHash = Animator.StringToHash("Attack");

        public void EnableIdleState()
        {
            animator.SetBool(idleHash, true);
        }

        public void DisableIdleState()
        {
            animator.SetBool(idleHash, false);
        }

        public void EnableRunState()
        {
            animator.SetBool(runHash, true);
        }

        public void DisableRunState()
        {
            animator.SetBool(runHash, false);
        }

        public void EnableAttackState()
        {
            animator.SetBool(attackHash, true);
        }

        public void DisableAttackState()
        {
            animator.SetBool(attackHash, false);
        }
    }
}