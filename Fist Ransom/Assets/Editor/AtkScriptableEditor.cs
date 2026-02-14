using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AtkScriptable))]
public class AtkScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw everything except the hidden conditional fields
        DrawPropertiesExcluding(serializedObject,
            "howManyTime",
            "nextAtk");

        SerializedProperty chainingProp =
            serializedObject.FindProperty("atkChaining");

        // Show fields conditionally
        if ((AtkChaining)chainingProp.enumValueIndex == AtkChaining.Repeat)
        {
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("howManyTime"));
        }

        if ((AtkChaining)chainingProp.enumValueIndex == AtkChaining.Domino)
        {
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("nextAtk"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
