using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

namespace Bap.Pool
{
    public class EnemyDeathPSPool : Pool<ParticleSystem>
    {
        [SerializeField] private ParticleSystem _deathPSPrefab;
        
        public override ParticleSystem OnCreateInstance()
        {
            var particle = Object.Instantiate(_deathPSPrefab);
            _list.Add(particle);
            return particle;
        }

        public override void GetInstance(ParticleSystem instance)
        {
            instance.gameObject.SetActive(true);
            instance?.Stop();
            instance?.Play();
            DOVirtual.DelayedCall(instance.main.startLifetime.constant, () => _myPool.Release(instance));
        }

        public override void ReleaseInstance(ParticleSystem instance)
        {
            instance.gameObject.SetActive(false);
        }

        public override void OnDestroyInsance(ParticleSystem instance)
        {
            _list.Remove(instance);
            Destroy(instance.gameObject);
        }
    }
}