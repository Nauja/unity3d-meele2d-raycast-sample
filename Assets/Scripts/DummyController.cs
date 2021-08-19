using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>This is our training dummy</summary>
    public class DummyController : MonoBehaviour, IEntity
    {
        [SerializeField]
        private Animator _animator;
        /// <summary>Text to display hit counter</summary>
        /// <remarks>Should probably be in a separate HUD class</remarks>
        [SerializeField]
        private Text _hitCounterText;
        /// <summary>Delay before resetting the hit counter</summary>
        [SerializeField]
        private float _resetHitCounterDelay;
        /// <summary>Number of hit received</summary>
        private int _hitCounter;
        /// <summary>Remaining delay before resetting the hit counter</summary>
        private float _resetHitCounterRemaining;

        private void Awake()
        {
            _hitCounterText.text = "";
        }

        private void Update()
        {
            // Reset and hide hit counter after some delay
            if (_resetHitCounterRemaining > 0.0f)
            {
                _resetHitCounterRemaining -= Time.deltaTime;
                if (_resetHitCounterRemaining <= 0.0f)
                {
                    _hitCounter = 0;
                    _hitCounterText.text = "";
                }
            }
        }

        public void OnHitBy(AttackPhase attackPhase, AttackCollision attackCollision)
        {
            // Increment hit counter and reset timer
            _hitCounter++;
            _hitCounterText.text = $"{_hitCounter} hit";
            _resetHitCounterRemaining = _resetHitCounterDelay;

            // Trigger visual and audio effects
            _animator.SetTrigger("Guard");
            AudioManager.singleton.Play(attackPhase.attackId);
            EffectManager.singleton.Play(attackPhase.attackId, attackCollision.position);
        }
    }
}