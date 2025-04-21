using System;
using Bap.Manager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bap.UI
{
    public class Button : MonoBehaviour
    {
        private Animator _anim;

        private void Awake()
        {
            if (_anim == null) _anim = GetComponent<Animator>();
        }

        public void LoadScene(string sceneName)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
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