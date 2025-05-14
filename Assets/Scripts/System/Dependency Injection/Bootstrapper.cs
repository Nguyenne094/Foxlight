using Bap.Service_Locator;
using UnityEngine;
using Utilities;
using NotImplementedException = System.NotImplementedException;

namespace Bap.DependencyInjection
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Injector))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        private Injector container;
        internal Injector Container => container.OrNull() ?? (container = GetComponent<Injector>());

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