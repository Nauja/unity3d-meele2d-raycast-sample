using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct EffectData
    {
        public EAttackId attackId;
        public AnimationClip clip;
    }

    /// <summary>Pool of effects</summary>
    /// <remarks>Demonstrate how to pool game objects for playing attack effects</remarks>
    public class EffectManager : MonoBehaviour
    {
        private static EffectManager _singleton;
        public static EffectManager singleton
        {
            get { return _singleton; }
        }

        /// <summary>Prefab for spawning effects</summary>
        [SerializeField]
        private GameObject _effectPrefab;
        /// <summary>Registered effects</summary>
        [SerializeField]
        private EffectData[] _effects;
        /// <summary>Pool for reusing effects</summary>
        private Queue<EffectController> _pool = new Queue<EffectController>();
        
        private void Awake()
        {
            _singleton = this;
        }

        private void OnDestroy()
        {
            _singleton = null;
        }

        /// <summary>Play an effect at given position</summary>
        public void Play(EAttackId attackId, Vector3 position)
        {
            // Take from pool
            EffectController effect = null;
            if (_pool.Count == 0)
            {
                effect = Instantiate(_effectPrefab, transform).GetComponent<EffectController>();
            }
            else
            {
                effect = _pool.Dequeue();
            }

            effect.transform.position = position;
            
            // Search the right effect to play
            foreach (EffectData _ in _effects)
            {
                if (_.attackId == attackId)
                {
                    effect.Play(_.clip);
                    break;
                }
            }
        }

        /// <summary>Called when an effect is done playing</summary>
        public void OnPlayed(EffectController effect)
        {
            // Return to pool
            _pool.Enqueue(effect);
        }
    }
}