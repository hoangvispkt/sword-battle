using UnityEngine;

public class keoT : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        isDragging = false; 
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPos();
            transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
