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
        public override Log OnCreate()
        {
            return default;
        }

        public override void Get(Log instance)
        {
            instance.gameObject.SetActive(true);
        }

        public override void Release(Log instance)
        {
            instance.gameObject.SetActive(false);
            DOVirtual.DelayedCall(instance.RecreateAfter, () => Get(instance), true);
        }
    }
}