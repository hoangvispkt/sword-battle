using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttributeUI))]
public class AttributeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttributeUI attributeUI = (AttributeUI)target;

        DrawDefaultInspector();

        // Button to add a new attribute
        if (GUILayout.Button("Add Attribute"))
        {
            attributeUI.AddOption();
        }

        // List and manage all attributes
        for (int i = 0; i < attributeUI.attributes.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Draw the Attribute label and dropdown
            EditorGUILayout.LabelField("Attribute", GUILayout.Width(70));
            attributeUI.attributes[i].attribute = (WeaponAttribute)EditorGUILayout.EnumPopup(attributeUI.attributes[i].attribute, GUILayout.Width(100));

            // Draw the Value label and input field
            EditorGUILayout.LabelField("Value", GUILayout.Width(50));
            attributeUI.attributes[i].value = EditorGUILayout.FloatField(attributeUI.attributes[i].value, GUILayout.Width(100));

            // Draw the Delete button
            if (GUILayout.Button("Delete", GUILayout.Width(50)))
            {
                attributeUI.attributes.RemoveAt(i);
                break; // Exit the loop after deletion to avoid index errors
            }

            EditorGUILayout.EndHorizontal();
        }

        // Apply changes made in the Inspector
        if (GUI.changed)
        {
            EditorUtility.SetDirty(attributeUI);
        }
    }
}
