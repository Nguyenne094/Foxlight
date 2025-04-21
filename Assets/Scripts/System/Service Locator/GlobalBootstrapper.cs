using UnityEngine;

namespace Bap.Service_Locator
{
    public class GlobalBootstrapper : Bootstrapper
    {
        [SerializeField] bool donDestroyOnLoad;
        
        public override void Bootstrap()
        {
            Container.ConfigureAsGlobal(donDestroyOnLoad);
        }
    }
}