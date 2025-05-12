using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Bap.Pool
{
    /// <summary>
    /// Because Logs is setuped and don't need to create new, so this pool just use Release and Get method
    /// </summary>
    public class LogPool : Pool<Log>
    {
        public override Log OnCreateInstance()
        {
            Debug.LogWarning("No need to create new Log instance anymore");
            return null;
        }

        public override void GetInstance(Log instance)
        {
            instance.gameObject.SetActive(true);
        }

        public override void ReleaseInstance(Log instance)
        {
            instance.gameObject.SetActive(false);
            DOVirtual.DelayedCall(instance.RecreateAfter, () => GetInstance(instance), true);
        }

        public override void OnDestroyInsance(Log instance)
        {
            Debug.LogWarning("You can't destroy Log instace");
        }
    }
}