using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using Utilities;

namespace Bap.Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioSource _audioSourceSFX;

        private ObjectPool<AudioSource> _pool;

        private void Start()
        {
            _pool = new ObjectPool<AudioSource>((() => Instantiate(_audioSourceSFX)), GetAudioSource,
                ReleaseAudioSource);
        }
        
        private void GetAudioSource(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(true);

            DOVirtual.DelayedCall(audioSource.clip.length, () => _pool.Release(audioSource));
        }

        private void ReleaseAudioSource(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(false);
        }

        public void PlaySFXClip(AudioClip clip, float volume)
        {
            AudioSource audioSource = _pool.Get();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }

        // public void ToggleMasterVolume(bool value)
        // {
        //     _mixer.SetFloat("MasterVolume", Mathf.Log10(value ? 1 : 0) * 20f);
        // }
        //
        // public void ToggleMusicVolume(bool value)
        // {
        //     _mixer.SetFloat("MusicVolume", Mathf.Log10(value ? 1 : 0) * 20f);
        // }
        //
        // public void ToggleSFXVolume(bool value)
        // {
        //     _mixer.SetFloat("SFXVolume", Mathf.Log10(value ? 1 : 0) * 20f);
        // }

        public void SetMasterVolume(float value)
        {
            _mixer.SetFloat("MasterVolume", value);
        }

        public void SetMusicVolume(float value)
        {
            _mixer.SetFloat("MusicVolume", value);
        }

        public void SetSFXVolume(float value)
        {
            _mixer.SetFloat("SFXVolume", value);
        }

        public void ResetToDefault()
        {
            SetMasterVolume(0);
            SetMusicVolume(0);
            SetSFXVolume(0);
        }
    }
}