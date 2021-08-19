using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Help control an effect displayed ingame</summary>
    public class EffectController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        private AnimatorOverrideController _animatorOverride;

        private void Awake()
        {
            _animatorOverride = new AnimatorOverrideController();
            _animatorOverride.runtimeAnimatorController = _animator.runtimeAnimatorController;
            _animator.runtimeAnimatorController = _animatorOverride;
        }

        private void Update()
        {
            // Return to pool when animation is done
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
            {
                EffectManager.singleton.OnPlayed(this);
                gameObject.SetActive(false);
            }
        }

        /// <summary>Play an animation</summary>
        public void Play(AnimationClip clip)
        {
            // Replace animation to play
            gameObject.SetActive(true);
            _animatorOverride["HitPunchAnimation"] = clip;
        }
    }
}