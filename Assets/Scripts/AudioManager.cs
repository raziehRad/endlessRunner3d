
    using System;
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _itemSound;
        [SerializeField] private AudioClip _gameOverSound;

        public void Awake()
        {
            instance = this;
        }

        public void PlaySound(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }

        public void PlayJumpSound() => PlaySound(_jumpSound);
        public void PlayItemSound(AudioClip audioClip) => PlaySound(audioClip);
        public void PlayGameOverSound() => PlaySound(_gameOverSound);
    }
