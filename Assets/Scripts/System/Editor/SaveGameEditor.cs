using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Bap.Save
{
    [CustomEditor(typeof(SaveGame))]
    [CanEditMultipleObjects]
    public class SaveGameEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var saveGame = (SaveGame)target;

            if (GUILayout.Button("Open in File Explorer"))
            {
                Process.Start(new ProcessStartInfo(Application.persistentDataPath));
            }
            
            // if (GUILayout.Button("Save"))
            // {
            //     if (string.IsNullOrEmpty(saveGame.PlayerDataFileName))
            //     {
            //         EditorUtility.DisplayDialog("Error", "File name cannot be empty", "OK");
            //         return;
            //     }
            //     try
            //     {
            //         saveGame.Save(saveGame.PlayerData, saveGame.PlayerDataFileName, true);
            //         EditorUtility.DisplayDialog("Success", $"Saved to {saveGame.PlayerDataFileName}.json", "OK");
            //     }
            //     catch (Exception e)
            //     {
            //         EditorUtility.DisplayDialog("Error", $"Failed to save: {e.Message}", "OK");
            //     }
            // }
            //
            // if (GUILayout.Button("Load"))
            // {
            //     if (string.IsNullOrEmpty(saveGame.PlayerDataFileName))
            //     {
            //         EditorUtility.DisplayDialog("Error", "File name cannot be empty", "OK");
            //         return;
            //     }
            //     try
            //     {
            //         saveGame.PlayerData = saveGame.Load<PlayerData>(saveGame.PlayerDataFileName);
            //         EditorUtility.DisplayDialog("Success", $"Loaded from {saveGame.PlayerDataFileName}.json", "OK");
            //     }
            //     catch (Exception e)
            //     {
            //         EditorUtility.DisplayDialog("Error", $"Failed to load: {e.Message}", "OK");
            //     }
            // }

            if (GUILayout.Button("Delete All Files"))
            {
                if (EditorUtility.DisplayDialog("Alert !!!", "Do you want to delete all save files", "Yes", "No"))
                {
                    saveGame.DeleteAll();
                }
            }
        }
    }
}