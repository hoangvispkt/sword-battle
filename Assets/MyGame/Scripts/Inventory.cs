using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot slotPrefab; // Prefab for the slot
    public int width;
    public int height;
    public float slotSize = 50f; // Size of each slot in pixels

    public Slot[,] slots; // 2D array to manage slots

    private void Start()
    {
        slots = new Slot[width, height];
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Instantiate slot prefab and set its position
                Slot slot = Instantiate(slotPrefab, transform);
                slot.transform.localPosition = new Vector3(x * slotSize, -y * slotSize, 0);
                slot.x = x;
                slot.y = y;
                slots[x, y] = slot;
            }
        }
    }

    public bool AddItem(Item item, int startX, int startY)
    {
        if (CheckSpace(item, startX, startY))
        {
            PlaceItem(item, startX, startY);
            return true;
        }
        return false;
    }

    private bool CheckSpace(Item item, int startX, int startY)
    {
        foreach (var offset in item.occupiedSlots)
        {
            int x = startX + offset.x;
            int y = startY + offset.y;
            if (x < 0 || x >= width || y < 0 || y >= height || !slots[x, y].IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    private void PlaceItem(Item item, int startX, int startY)
    {
        foreach (var offset in item.occupiedSlots)
        {
            int x = startX + offset.x;
            int y = startY + offset.y;
            slots[x, y].AddItem(item);
        }
    }
}
