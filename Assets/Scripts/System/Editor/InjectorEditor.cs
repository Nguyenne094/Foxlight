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

            injector.ValidateOnSceneLoaded = GUILayout.Toggle(injector.ValidateOnSceneLoaded, "Validate On Scene Group Loaded");

            GUILayout.Space(20);

            if (GUILayout.Button("Validate"))
            {
                injector.Validate();
            }

            if (GUILayout.Button("Set Up"))
            {   
                var monoBehaviours = injector.GetAllMonoBehaviours();
                foreach (var b in monoBehaviours)
                {
                    Debug.Log(b.GetType().Name);
                    injector.RegisterProvidedDependencies(b);
                }
            }
        }
    }
}