using System;
using Bap.Service_Locator;
using DG.Tweening;
using PlatformingGame.Controller;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utilities;


[RequireComponent(typeof(Collider2D))]
public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneGroupDataSO _sceneGroup;
    [SerializeField] private Transform _spawnPos;
    [SerializeField, Tooltip("While Player spawns at spawn position, then must go to this destination position")] 
    private Transform _destinationPos;
    
    private PlayerController _player;
    private Collider2D _col;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    public void OnSceneGroupLoaded()
    {
        ServiceLocator.Global.Get<PlayerController>(out _player);
        _player.transform.position = _spawnPos.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _sceneGroup != null)
        {
            if (_sceneGroup == null)
            {
                Debug.LogError("SceneGroupData is null");
                return;
            }

            SceneLoader.Instance.LoadSceneGroup(_sceneGroup).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to load scene group: " + task.Exception);
                }
            });
        }
    }
}