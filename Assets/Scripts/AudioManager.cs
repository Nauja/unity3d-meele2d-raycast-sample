using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct AudioData
    {
        public EAttackId attackId;
        public AudioSource audio;
    }

    /// <summary>Used to play audio</summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _singleton;
        public static AudioManager singleton
        {
            get { return _singleton; }
        }

        /// <summary>Registered audios</summary>
        [SerializeField]
        private AudioData[] _audios;

        private void Awake()
        {
            _singleton = this;
        }

        private void OnDestroy()
        {
            _singleton = null;
        }

        /// <summary>Play audio for an attack</summary>
        public void Play(EAttackId attackId)
        {
            foreach (AudioData _ in _audios)
            {
                if (_.attackId == attackId)
                {
                    _.audio.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
                    _.audio.Play();
                    break;
                }
            }
        }
    }
}