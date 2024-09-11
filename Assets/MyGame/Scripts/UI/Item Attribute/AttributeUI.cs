using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponAttribute
{
    DAMAGE,
    HP,
    MP,
    ATTACK_SPEED,
    CRIT,
    ARMOR,
    STR,
    AGI,
    INT,
    DEX,
    LUCK,

}

[System.Serializable]
public class AttributeOption
{
    public WeaponAttribute attribute;
    public float value;
}

public class AttributeUI : MonoBehaviour
{
    [HideInInspector]
    public List<AttributeOption> attributes = new List<AttributeOption>();

    // This function will be called when "Add Option" button is clicked
    public void AddOption()
    {
        attributes.Add(new AttributeOption());
    }
}