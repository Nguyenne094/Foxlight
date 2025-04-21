using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Bap.Manager
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
        }
    }
}