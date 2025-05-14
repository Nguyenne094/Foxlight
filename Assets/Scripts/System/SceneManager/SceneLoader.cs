using System.Threading.Tasks;
using UnityEngine;
using Utilities;

/// <summary>
/// Handles the loading of scene groups with progress tracking and a loading canvas.
/// </summary>
[RequireComponent(typeof(SceneGroupManager))]
public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private SceneGroupManager _sceneGroupManager;
    public SceneGroupManager SceneGroupManager => _sceneGroupManager; 

    public override void Awake()
    {
        base.Awake();
        _sceneGroupManager = GetComponent<SceneGroupManager>();
    }

    /// <summary>
    /// Loads a scene group asynchronously with progress tracking.
    /// </summary>
    /// <param name="data">The scene group data to load.</param>
    public async Task LoadSceneGroup(SceneGroupDataSO data)
    {
        float currentProgress = 0;

        if (data == null)
        {
            Debug.LogError("[Scene Manager] Error: SceneGroupData is null");
            return;
        }

        // Tracks the progress of the scene loading process.
        ProgressInformation progress = new ProgressInformation();
        progress.OnProgressChanged += target => currentProgress = Mathf.Min(target, 1);

        EnableCanvas(); // Enable the loading canvas.
        await ImplementLoadingSceneProgress(); // Display loading progress (to be implemented).
        await SceneGroupManager.LoadSceneGroup(data, progress); // Load the scene group.
        EnableCanvas(false); // Disable the loading canvas.
    }

    /// <summary>
    /// Displays the loading progress during the scene loading process.
    /// </summary>
    private async Task ImplementLoadingSceneProgress()
    {
        await Task.Delay(0);
        // TODO: Implement loading scene progress.
    }

    /// <summary>
    /// Enables or disables the loading canvas.
    /// </summary>
    /// <param name="enable">True to enable the canvas, false to disable it.</param>
    public void EnableCanvas(bool enable = true)
    {
        _loadingCanvas.gameObject.SetActive(enable);
    }
}