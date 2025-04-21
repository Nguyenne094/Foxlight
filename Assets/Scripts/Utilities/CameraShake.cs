using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using Utilities;

namespace Utilities
{
    public class CameraShake : Singleton<CameraShake>
    {
        [SerializeField] private NoiseSettings _superLightShake;
        [SerializeField] private NoiseSettings _lightShake;
        [SerializeField] private NoiseSettings _mediumShake;
        [SerializeField] private NoiseSettings _heavyShake;
        
        private CinemachineBasicMultiChannelPerlin _cine;

        public override void Awake()
        {
            base.Awake();
            _cine = GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void SuperLightShake(float duration)
        {
            DOVirtual.Float(0f, 1f, duration, (value) =>
                {
                    _cine.NoiseProfile = _superLightShake;
                })
                .OnComplete(() => _cine.NoiseProfile = null);
        }

        public void LightShake(float duration)
        {
            DOVirtual.Float(0f, 1f, duration, (value) =>
                {
                    _cine.NoiseProfile = _lightShake;
                })
                .OnComplete(() => _cine.NoiseProfile = null);
        }
        
        public void MediumShake(float duration)
        {
            DOVirtual.Float(0f, 1f, duration, (value) =>
                {
                    _cine.NoiseProfile = _mediumShake;
                })
                .OnComplete(() => _cine.NoiseProfile = null);
        }
        
        public void HeavyShake(float duration)
        {
            DOVirtual.Float(0f, 1f, duration, (value) =>
                {
                    _cine.NoiseProfile = _heavyShake;
                })
                .OnComplete(() => _cine.NoiseProfile = null);
        }
    }
}