using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Attribute))]
public class MyComponentEditor : Editor
{
    // List of available options for the dropdown
    private List<string> availableOptions = new List<string> { "DAMAGE", "HP", "MANA" };

    public override void OnInspectorGUI()
    {
        Attribute myComponent = (Attribute)target;

        // Display existing options and values
        for (int i = 0; i < myComponent.optionValuePairs.Count; i++)
        {
            var pair = myComponent.optionValuePairs[i];

            // Create a dropdown for options
            int selectedIndex = availableOptions.IndexOf(pair.option);
            if (selectedIndex == -1)
            {
                selectedIndex = 0;
            }
            selectedIndex = EditorGUILayout.Popup($"Option {i + 1}", selectedIndex, availableOptions.ToArray());

            // Set the selected option back to the pair
            pair.option = availableOptions[selectedIndex];

            // TextField for the value associated with the option
            pair.value = EditorGUILayout.TextField($"Value {i + 1}", pair.value);
        }

        // Display a button to add the next available option to the list
        if (GUILayout.Button("Add Option"))
        {
            // Add the next available option that is not already in the list
            foreach (string option in availableOptions)
            {
                if (!myComponent.optionValuePairs.Exists(p => p.option == option))
                {
                    myComponent.AddOptionValuePair(option);
                    break;
                }
            }
        }

        // Save any changes back to the target object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
