using System;
using Bap.Manager;
using DG.Tweening;
using PlatformingGame.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Vector2 = System.Numerics.Vector2;


[RequireComponent(typeof(Collider2D))]
public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private string _loadingScene;
    [SerializeField] private Transform _spawnPos;
    [SerializeField, Tooltip("While Player spawns at spawn position, then must go to this destination position")] private Transform _destinationPos;
    
    private Collider2D _col;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        if (_col == null)
        {
            Debug.LogError($"Require Collider2D for {gameObject.name} works correctly");
        }

        if (_player == null)
            _player = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !SceneManager.GetSceneByName("_loadingScene").isLoaded)
        {
            AsyncOperation loadOp = new AsyncOperation();
            AsyncOperation unloadOp = new AsyncOperation();

            loadOp = SceneManager.LoadSceneAsync(_loadingScene, LoadSceneMode.Additive);
            unloadOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            loadOp.completed += (operation => ForcePlayerMoveToDestination());
        }
    }

    private void ForcePlayerMoveToDestination()
    {
        _player.CanControl = false;
        DOTween.Sequence()
            .AppendCallback((() =>
                _player.Rb.linearVelocityX = (Utils.GetDirectionVector2(_spawnPos.position, _destinationPos.position) *
                                              _player.ConStat.MoveSpeed).x))
            .AppendCallback((() => _player.CanControl = true));
    }
}