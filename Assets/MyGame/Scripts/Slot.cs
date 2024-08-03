using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item currentItem;
    public Image itemImage;
    public int x;
    public int y;

    public void AddItem(Item item)
    {
        currentItem = item;
        itemImage.sprite = item.icon;
        itemImage.enabled = true;
    }

    public void RemoveItem()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }
}
