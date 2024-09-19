using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager: MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<AttributeOption> GetAttributes(GameObject weapon)
    {
        Item item = weapon.GetComponent<Item>();
        return item?.attributeUI?.attributes ?? new List<AttributeOption>();
    }

    public float GetValue(List<AttributeOption> attributes, WeaponAttribute attribute)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].attribute == attribute)
            {
                return attributes[i].value;
            }
        }

        return 0f;
    }
}
