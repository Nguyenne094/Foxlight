using System;
using System.Threading.Tasks;
using Bap.DependencyInjection;
using Bap.EventChannel;
using Bap.Manager;
using Bap.Save;
using Bap.Service_Locator;
using Bap.System.Health;
using PlatformingGame.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bap.UI
{
    using SceneManager = UnityEngine.SceneManagement.SceneManager;
    public class Button : MonoBehaviour
    {
        private Animator _anim;
        [Inject]
        private PlayerController _playerController;

        private void Awake()
        {
            if (_anim == null) _anim = GetComponent<Animator>();
        }

        public void RaiseEvent(VoidEventChannelSO @event)
        {
            @event.RaiseEvent();
        }

        public async void LoadScene(SceneGroupDataSO sceneGroup)
        {
            if (sceneGroup == null)
            {
                Debug.LogError("SceneGroupData is null");
                return;
            }

            await SceneLoader.Instance.LoadSceneGroup(sceneGroup).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to load scene group: " + task.Exception);
                }
            });
            await Task.Yield();
        }
    }
}