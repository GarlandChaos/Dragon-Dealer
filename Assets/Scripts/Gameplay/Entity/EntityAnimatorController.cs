using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class EntityAnimatorController : MonoBehaviour
    {
        private bool initialized = false;
        [SerializeField] private Animator animator = null;
        protected AnimatorOverrideController animatorOverrideController = null;
        protected AnimationClipOverrides clipOverrides = null;
        private int idleHash = Animator.StringToHash("Idle");
        private int runHash = Animator.StringToHash("Run");
        private int attackHash = Animator.StringToHash("Attack");

        public void Initialize()
        {
            if (initialized) return;

            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;

            clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);

            initialized = true;
        }

        public void SetAnimatorController(EntityAnimationClips animationClips)
        {
            if (!initialized)
                Initialize();

            clipOverrides["Idle"] = animationClips.Idle;
            clipOverrides["Run"] = animationClips.Run;
            clipOverrides["Attack"] = animationClips.Attack;
            animatorOverrideController.ApplyOverrides(clipOverrides);
            DisableAttackState();
            DisableRunState();
            EnableIdleState();
        }

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

    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }

    [Serializable]
    public class EntityAnimationClips
    {
        public AnimationClip Idle = null;
        public AnimationClip Run = null;
        public AnimationClip Attack = null;
    }
}