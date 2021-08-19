using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>This is our training dummy</summary>
    public class DummyController : MonoBehaviour, IEntity
    {
        [SerializeField]
        private Animator _animator;
        /// <summary>Delay before resetting the hit counter</summary>
        [SerializeField]
        private float _resetHitCounterDelay;
        /// <summary>Number of hit received</summary>
        private int _hitCounter;
        /// <summary>Remaining delay before resetting the hit counter</summary>
        private float _resetHitCounterRemaining;

        private void Update()
        {
            if (_resetHitCounterRemaining > 0.0f)
            {
                _resetHitCounterRemaining -= Time.deltaTime;
                if (_resetHitCounterRemaining <= 0.0f)
                {
                    _hitCounter = 0;
                }
            }
        }

        public void OnHitBy(AttackPhase attackPhase, AttackCollision attackCollision)
        {
            _hitCounter++;
            _animator.SetTrigger("Guard");
            AudioManager.singleton.Play(attackPhase.attackId);
            EffectManager.singleton.Play(attackPhase.attackId, attackCollision.position);
        }
    }
}