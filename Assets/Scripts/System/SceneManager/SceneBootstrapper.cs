using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

using SceneManager = UnityEngine.SceneManagement.SceneManager;
public class SceneBootstrapper : Singleton<SceneBootstrapper>
{
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // public static void Bootstrap()
    // {
    //     Debug.Log("Scene Bootstrapper Initializing ...");
    //     SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Additive);
    // }
}