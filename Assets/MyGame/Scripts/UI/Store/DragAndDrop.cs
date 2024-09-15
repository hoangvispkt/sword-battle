using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private bool shouldCheckCollision = false;
    private bool isPlaced = false; // New flag to indicate that the object has been placed
    private Vector3 offset;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private float dragSpeed = 10f; // Tốc độ kéo để điều chỉnh

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Đảm bảo rằng object có Rigidbody2D
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void OnMouseDown()
    {
        // Prevent further dragging if the object is already placed
        if (isPlaced) return;

        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        shouldCheckCollision = false;

        // Set Rigidbody2D to Kinematic mode while dragging
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnMouseUp()
    {
        // Prevent further dragging if the object is already placed
        if (isPlaced) return;

        isDragging = false;

        // Reset Rigidbody2D to Dynamic mode after dragging
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Ensure that the object doesn't suddenly "fall" too fast
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Set the object to the last detected target position
        if (shouldCheckCollision)
        {
            transform.position = targetPosition;

            // Mark the object as placed and disable further dragging
            isPlaced = true;

            // Keep the Rigidbody in Kinematic mode to prevent further movement
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void Update()
    {
        // Prevent further dragging if the object is already placed
        if (isPlaced) return;

        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPos();
            Vector3 targetPos = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);

            // Smooth movement to avoid sudden jumps
            rb.MovePosition(Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime));
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
        if (collision.CompareTag("sword"))
        {
            // Store the position of the sword
            targetPosition = collision.transform.position;

            // Set the flag to true, so the position will be updated when the mouse is released
            shouldCheckCollision = true;
        }
    }
}
