using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionValuePair
{
    public string option;
    public string value;
}

public class Attribute : MonoBehaviour
{
    public List<OptionValuePair> optionValuePairs = new List<OptionValuePair>
    {
        new OptionValuePair { option = "Option 1", value = "" },
        new OptionValuePair { option = "Option 2", value = "" },
        new OptionValuePair { option = "Option 3", value = "" }
    };

    // Phương thức để thêm option và value mới
    public void AddOptionValuePair(string option)
    {
        optionValuePairs.Add(new OptionValuePair { option = option, value = "" });
    }
}
