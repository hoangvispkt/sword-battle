using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttributeUI))]
public class AttributeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttributeUI attributeScript = (AttributeUI)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Add Attribute"))
        {
            attributeScript.AddOption();
        }

        for (int i = 0; i < attributeScript.attributes.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Draw the Attribute label and dropdown
            EditorGUILayout.LabelField("Attribute", GUILayout.Width(70));
            attributeScript.attributes[i].attribute = (WeaponAttribute)EditorGUILayout.EnumPopup(attributeScript.attributes[i].attribute, GUILayout.Width(100));

            // Draw the Value label and input field
            EditorGUILayout.LabelField("Value", GUILayout.Width(50));
            attributeScript.attributes[i].value = EditorGUILayout.FloatField(attributeScript.attributes[i].value, GUILayout.Width(100));

            // Draw the Delete button
            if (GUILayout.Button("Delete", GUILayout.Width(50)))
            {
                attributeScript.attributes.RemoveAt(i);
                break; // Exit the loop after deletion to avoid index errors
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}