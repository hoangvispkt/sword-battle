using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Inventory inventory;
    private Item currentItem;
    public Canvas canvas; // Reference to the canvas

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventory = FindObjectOfType<Inventory>();
    }

    public void SetItem(Item item)
    {
        currentItem = item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(inventory.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition);

        int x = Mathf.FloorToInt(localPointerPosition.x / inventory.slotSize);
        int y = Mathf.FloorToInt(-localPointerPosition.y / inventory.slotSize); // Note: invert y to match Unity's coordinate system

        if (!inventory.AddItem(currentItem, x, y))
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
