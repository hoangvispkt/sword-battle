using UnityEngine;

public class PlusIndex : MonoBehaviour
{
    public int Hp = 0;
    public int Damage = 0;
    public int Mp = 0;
    public int Crit = 0;

    public void UpdateAttributes(Attribute attributeComponent)
    {
        foreach (AttributeOption option in attributeComponent.attributes)
        {
            switch (option.attribute)
            {
                case WeaponAttribute.HP:
                    Hp += (int)option.value;  // Add value instead of replacing
                    break;
                case WeaponAttribute.DAMAGE:
                    Damage += (int)option.value;  // Add value instead of replacing
                    break;
                case WeaponAttribute.MP:
                    Mp += (int)option.value;  // Add value instead of replacing
                    break;
                case WeaponAttribute.CRIT:
                    Crit += (int)option.value;  // Add value instead of replacing
                    break;
                    // Add cases for other attributes as needed
            }
        }

        // Update GameManager with the new values
        GameManager.Instance.UpdateTotalAttributes(Hp, Damage, Mp, Crit);

        Debug.Log("Attributes updated!");
        DisplayCurrentAttributes(); // Display the updated attributes
    }

    public void DisplayCurrentAttributes()
    {
        Debug.Log($"Cập nhập: HP = {Hp}, Damage = {Damage}, MP = {Mp}, Crit = {Crit}");
    }
}
