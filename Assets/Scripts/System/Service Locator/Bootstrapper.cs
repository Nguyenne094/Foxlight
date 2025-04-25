using UnityEngine;
using Utilities;
using Bap.Service_Locator;

namespace Bap.Service_Locator
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        private ServiceLocator container;
        internal ServiceLocator Container => container.OrNull() ?? (container = GetComponent<ServiceLocator>());

        private bool hasBeenBootstrapped;
        
        private void Awake() => BootstrapOnDemand();

        public void BootstrapOnDemand()
        {
            if (hasBeenBootstrapped) return;
            hasBeenBootstrapped = true;
            Bootstrap();
        }

        public abstract void Bootstrap();
    }
}