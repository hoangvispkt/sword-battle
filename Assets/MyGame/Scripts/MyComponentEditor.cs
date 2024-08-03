using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Attribute))]
public class MyComponentEditor : Editor
{
    private string newOption = "";

    public override void OnInspectorGUI()
    {
        Attribute myComponent = (Attribute)target;

        // Display existing options and values
        for (int i = 0; i < myComponent.optionValuePairs.Count; i++)
        {
            var pair = myComponent.optionValuePairs[i];
            pair.option = EditorGUILayout.TextField($"Option {i + 1}", pair.option);
            pair.value = EditorGUILayout.TextField($"Value {i + 1}", pair.value);
        }

        // Display a textbox to input new option
        newOption = EditorGUILayout.TextField("New Option", newOption);

        // Display a button to add the new option
        if (GUILayout.Button("Add Option"))
        {
            if (!string.IsNullOrEmpty(newOption))
            {
                myComponent.AddOptionValuePair(newOption);
                newOption = ""; // Clear the input field
            }
        }

        // Save any changes back to the target object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
