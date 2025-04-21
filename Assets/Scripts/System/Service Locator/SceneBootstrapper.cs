namespace Bap.Service_Locator
{
    public class SceneBootstrapper : Bootstrapper
    {
        public override void Bootstrap()
        {
            Container.ConfigureForScene();
        }
    }
}