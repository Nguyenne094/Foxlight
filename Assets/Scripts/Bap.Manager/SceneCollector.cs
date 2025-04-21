using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Bap.Manager
{
    public class SceneCollector : Singleton<SceneCollector>
    {
        [SerializeField] private List<string> _sceneList;
        public List<string> SceneList { get => _sceneList; set => _sceneList = value; }
    }
}