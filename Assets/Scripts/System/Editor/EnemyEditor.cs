using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bap.System.Health.EnemyHealth)), CanEditMultipleObjects]
public class EnemyEditor : Editor
{
    SerializedProperty _isInvincible;
    SerializedProperty _onTakeDamage;
    SerializedProperty _onDeath;

    private void OnEnable()
    {
        _isInvincible = serializedObject.FindProperty("_isInvincible");
        _onTakeDamage = serializedObject.FindProperty("OnTakeDamage");
        _onDeath = serializedObject.FindProperty("OnDeath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_isInvincible);

        if (!_isInvincible.boolValue)
        {
            EditorGUILayout.PropertyField(_onTakeDamage);
            EditorGUILayout.PropertyField(_onDeath);
        }
        
        DrawPropertiesExcluding(serializedObject, "_isInvincible", "OnTakeDamage", "OnDeath");

        serializedObject.ApplyModifiedProperties();
    }
}