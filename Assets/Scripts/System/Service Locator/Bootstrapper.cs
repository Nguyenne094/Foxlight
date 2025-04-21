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

        private bool hasbeenBootstrapped;

        private void Awake() => BootstrapOnDemand();

        public void BootstrapOnDemand()
        {
            if (hasbeenBootstrapped) return;
            hasbeenBootstrapped = true;
            Bootstrap();
        }

        public abstract void Bootstrap();
    }
}