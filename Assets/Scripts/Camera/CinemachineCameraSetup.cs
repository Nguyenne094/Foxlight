using System.Collections.Generic;
using Bap.Service_Locator;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraSetup : MonoBehaviour
{
    [SerializeField] private List<ConfinerForScene> _confinerForScenes;
    private CinemachineConfiner2D _cinemachineConfiner;
    
    public void SetupCameraBounds()
    {
        var _cameraBounds = _confinerForScenes.Find(confiner => confiner.Level == SceneLoader.Instance.SceneGroupManager.CurrentSceneGroup).Confiner;
        _cinemachineConfiner = GetComponent<CinemachineConfiner2D>();
        if (_cinemachineConfiner != null)
        {
            _cinemachineConfiner.BoundingShape2D.gameObject.SetActive(false);
        }
        _cameraBounds.gameObject.SetActive(true);
        _cinemachineConfiner.BoundingShape2D = _cameraBounds;
    }
}

[System.Serializable]
public struct ConfinerForScene
{
    public SceneGroupDataSO Level;
    public PolygonCollider2D Confiner;
}