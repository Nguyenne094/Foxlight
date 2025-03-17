using System;
using DG.Tweening;
using Others;
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
            return default;
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
    }
}