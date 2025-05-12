using Bap.DependencyInjection;
using UnityEditor;
using UnityEngine;

namespace Bap.DependencyInjection
{
    [CustomEditor(typeof(Injector))]
    public class InjectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var injector = (Injector)target;
            
            if(GUILayout.Button("Log Dependencies"))
            {
                injector.LogDependencies();
            }
        }
    }
}