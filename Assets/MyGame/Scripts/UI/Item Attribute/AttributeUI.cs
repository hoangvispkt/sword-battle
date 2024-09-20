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
    CRIT_DAMAGE,
    DODGE,
    ARMOR,
    STR,
    AGI,
    INT,
    DEX,
    LUCK,
    PRICE
}

[System.Serializable]
public class AttributeOption
{
    public WeaponAttribute attribute;
    public float value;
    public AttributeOption(WeaponAttribute attribute, float value)
    {
        this.attribute = attribute;
        this.value = value;
    }
}

[CreateAssetMenu(fileName = "NewWeaponAttributes", menuName = "Weapon/Attributes")]
public class AttributeUI : ScriptableObject
{
    [HideInInspector]
    public List<AttributeOption> attributes = new List<AttributeOption>();

    public void AddOption()
    {
        // Add a default attribute with a default value
        attributes.Add(new AttributeOption(WeaponAttribute.DAMAGE, 0f));
    }
}
