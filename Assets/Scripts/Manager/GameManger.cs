using System;
using Bap.System.Health;
using PlatformingGame.Controller;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class GameManger : Singleton<GameManger>
    {
        public Action OnGameStart;
        public Action OnGamePause;
        public Action OnGameExit;

        public Action OnPlayerDie;

        private Vector2 _lastCheckPoint;

        public Vector2 LastCheckPoint { get => _lastCheckPoint; set => _lastCheckPoint = value; }

        private void Start()
        {
            _lastCheckPoint = PlayerController.Instance.transform.position;
        }
    }
}