using System;
using Bap.DependencyInjection;
using Bap.Save;
using Bap.EventChannel;
using Bap.System.Health;
using Bap.Service_Locator;
using PlatformingGame.Controller;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Bap.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        private PlayerController _playerController;
        private Vector2 _lastCheckPoint;

        public Vector2 LastCheckPoint { get => _lastCheckPoint; set => _lastCheckPoint = value; }

        private void SavePlayerData()
        {
            if (_playerController != null)
            {
                SaveGame.Instance.Save(new PlayerData(PlayerController.Instance.transform.position, 
                        _lastCheckPoint, 
                        _playerController.GetComponent<PlayerHealth>().CurrentHealth), 
                    SaveGame.Instance.PlayerDataFileName); 
            }
            else
            {
                Debug.LogError("PlayerController is null. Cannot save player data.");
            }
        }

        private void OnApplicationQuit()
        {
            SavePlayerData();
        }

        [Provide]
        private GameManager Contruct() => this;

        [Inject]
        void Inject(PlayerController playerController)
        {
            _playerController = playerController;
        }
    }
}