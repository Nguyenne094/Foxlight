using System;
using System.Collections;
using System.Collections.Generic;
using Bap.DependencyInjection;
using Bap.Manager;
using Bap.Save;
using Bap.System.Health;
using Bap.Service_Locator;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace PlatformingGame.Controller
{
    public class PlayerController : Singleton<PlayerController>
    {
        public int num { get; set; }
        [SerializeField] private ControllerStatSO _conStat;
        [SerializeField] private Collider2D _col;
        
        private PlayerHealth _playerHealth;
        public PlayerHealth PlayerHealth => _playerHealth;
        public ControllerStatSO ConStat { get => _conStat; }
        

        public override void Awake()
        {
            base.Awake();
            _playerHealth = GetComponent<PlayerHealth>();
            _col ??= GetComponent<Collider2D>();
            LoadPlayerData();
        }

        [Provide]
        public PlayerController ProvideMyself() => this;
        
        
        private void LoadPlayerData()
        {
            PlayerData playerData = SaveGame.Instance.Load<PlayerData>(SaveGame.Instance.PlayerDataFileName);
            
            transform.position = playerData.CurrentPosition;
            _playerHealth.CurrentHealth = playerData.CurrentHealth;
            GameManager.Instance.LastCheckPoint = playerData.LastCheckPoint;
        }

        private void OnDrawGizmosSelected()
        {
            if (_col)
            {
                Gizmos.color = Color.red;
                Vector3 start = _col.bounds.center + new Vector3(0, -_col.bounds.size.y / 2);
                Vector3 end = start + new Vector3(0, -_conStat.GroundCheckDistance);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}