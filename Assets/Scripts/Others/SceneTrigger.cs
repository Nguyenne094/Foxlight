using System.Threading.Tasks;
using Bap.EventChannel;
using Bap.Service_Locator;
using PlatformingGame.Controller;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneGroupDataSO _sceneGroup;
    [SerializeField] private Transform _spawnPos;
    
    private PlayerController _player;
    private Collider2D _col;
    private void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _sceneGroup == null)
        {
            return;
        }

        SceneLoader.Instance.LoadSceneGroup(_sceneGroup).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to load SceneGroup: " + task.Exception);
            }
        });
    }
}