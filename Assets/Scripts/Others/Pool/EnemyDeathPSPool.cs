using DG.Tweening;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Bap.Pool
{
    public class EnemyDeathPSPool : Pool<ParticleSystem>
    {
        [SerializeField] private ParticleSystem _deathPSPrefab;
        
        
        public override ParticleSystem OnCreateInstance()
        {
            return Object.Instantiate(_deathPSPrefab);
        }

        public override void GetInstance(ParticleSystem instance)
        {
            instance.gameObject.SetActive(true);
            instance.Stop();
            instance.Play();
            DOVirtual.DelayedCall(instance.main.startLifetime.constant, () => ReleaseInstance(instance));
        }

        public override void ReleaseInstance(ParticleSystem instance)
        {
            instance.gameObject.SetActive(false);
        }
    }
}