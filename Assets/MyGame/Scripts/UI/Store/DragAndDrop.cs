using UnityEngine;
using System.Collections.Generic;

public class DragAndDrop : MonoBehaviour
{
    public bool isEquip = false;
    public bool isBuy = false;
    public bool isEnter = false;
    private Vector3 offset;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private float dragSpeed = 20f;
    private Vector3 currentPosition;

    private List<SpriteRenderer> enteredBags = new List<SpriteRenderer>();  // Danh sách các bag đã lướt qua

    public Status status;

    public enum Status
    {
        FREE, DRAGGING, RETURNING
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D không tồn tại trên GameObject. Hãy thêm nó qua Unity Editor.");
        }

        currentPosition = transform.position;
    }

    private void OnMouseDown()
    {
        status = Status.DRAGGING;
        isEquip = false;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        if (isEnter && !isEquip)
        {
            // Đảm bảo đối tượng dừng tại vị trí của "bag"
            rb.MovePosition(targetPosition);
            status = Status.FREE;
            isEquip = true;
            isBuy = true;

            // Đổi màu của tất cả các bag về bình thường
            ResetAllBagColors();
        }
        else
        {
            status = Status.RETURNING;
            isEquip = false;

            // Trả màu cho các bag khi không được gắn vào
            ResetAllBagColors();
        }
    }

    private void Update()
    {
        if (status == Status.DRAGGING)
        {
            Vector3 mousePos = GetMouseWorldPos();
            Vector3 targetPos = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);
            rb.MovePosition(Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime));
        }
        else if (status == Status.RETURNING)
        {
            if (isBuy) currentPosition = targetPosition;
            rb.MovePosition(Vector3.Lerp(transform.position, currentPosition, dragSpeed * Time.deltaTime));

            if (Vector3.Distance(transform.position, currentPosition) < 0.01f)
            {
                status = Status.FREE;
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bag"))
        {
            SpriteRenderer bagRenderer = collision.GetComponent<SpriteRenderer>();

            if (bagRenderer != null)
            {
                bagRenderer.color = Color.cyan;

                if (!enteredBags.Contains(bagRenderer))
                {
                    enteredBags.Add(bagRenderer);  // Thêm bag vào danh sách khi lướt qua
                }
            }

            targetPosition = collision.transform.position;
            isEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("bag") && !isEquip)
        {
            SpriteRenderer bagRenderer = collision.GetComponent<SpriteRenderer>();

            if (bagRenderer != null)
            {
                bagRenderer.color = Color.white;  // Reset màu khi thoát khỏi bag
                enteredBags.Remove(bagRenderer);  // Loại bỏ bag khỏi danh sách
            }

            isEnter = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("bag"))
        {
            isEnter = true;
        }
    }

    private void ResetAllBagColors()
    {
        foreach (SpriteRenderer bagRenderer in enteredBags)
        {
            if (bagRenderer != null)
            {
                bagRenderer.color = Color.white;
            }
        }

        enteredBags.Clear();  // Xóa danh sách các bag đã lướt qua
    }
}
