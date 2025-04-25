using System;
using Bap.Manager;
using Bap.Service_Locator;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bap.UI
{
    using SceneManager = UnityEngine.SceneManagement.SceneManager;
    public class Button : MonoBehaviour
    {
        private Animator _anim;

        private void Awake()
        {
            if (_anim == null) _anim = GetComponent<Animator>();
        }

        public void LoadScene(SceneGroupDataSO sceneGroup)
        {
            if (sceneGroup == null)
            {
                Debug.LogError("SceneGroupData is null");
                return;
            }

            SceneLoader.Instance.LoadSceneGroup(sceneGroup).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to load scene group: " + task.Exception);
                }
            });
        }

        public void LoadAddictiveSceneAsyncAndTransition(string sceneName)
        {
            //TODO: Loading Transition Here
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public void LoadAddictiveScenceAsync(string sceneName)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public void UnloadSceneAsync(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}