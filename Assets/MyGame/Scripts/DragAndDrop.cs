using Unity.VisualScripting;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Rigidbody2D rb;

    private Vector3 mouseStartPosition;
    private Vector3 mouseEndPosition;

    private int rotateCount = 0; // Biến đếm số lần xoay

    public Transform scabbardRect;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        // Ensure the object has a Rigidbody2D
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            // Calculate the offset between the mouse position and the object's position
            offset = transform.position - GetMouseWorldPosition();

            // Store the start position of the mouse
            mouseStartPosition = GetMouseWorldPosition();

            Debug.Log("OnMouseDown");
        }
    }

    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rotateCount = 0;

            // Store the end position of the mouse
            mouseEndPosition = GetMouseWorldPosition();

            // Calculate the direction and magnitude of the throw
            Vector3 throwDirection = mouseEndPosition - mouseStartPosition;

            // Apply the velocity to the Rigidbody2D
            rb.velocity = throwDirection * 1f; // Adjust the multiplier to control the speed of the throw

            Debug.Log("OnMouseUp");
        }
    }

    void Update()
    {
        if (isDragging)
        {
            // Move the object with the mouse
            transform.position = GetMouseWorldPosition() + offset;
            rb.velocity = Vector2.zero; // Reset velocity to prevent the object from moving due to physics while dragging

            // Set the rotation of the object to 0 on the Z axis
            if (rotateCount == 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            }

            // Check for right mouse button click to rotate 90 degrees
            if (Input.GetMouseButtonDown(1))
            {
                rotateCount++;
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90 * rotateCount);
                float newZRotation = -90 * rotateCount;
                transform.rotation = Quaternion.Euler(0, 0, newZRotation);
            }
        }



    }

    void FixedUpdate()
    {
        if (!isDragging)
        {
            // Add torque to make the object rotate
            rb.AddTorque(0.5f); // Adjust the value to control the rotation speed
            Debug.Log("Xoay");
        }
        else
        {
            rb.angularVelocity = 0; // Reset angular velocity when dragging to prevent unintended spinning
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10.0f; // Set this to be the distance from the camera to the object
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "kiem")
        {
            Debug.Log("Nhận được rồi");
            // Đặt vị trí của đối tượng tại vị trí của scabbardRect
            collision.transform.position = scabbardRect.transform.position;
            // Đảm bảo đối tượng đứng yên tại vị trí mới

            rb.velocity = Vector2.zero;       // Ngừng chuyển động
            rb.angularVelocity = 0f;          // Ngừng xoay
            Debug.Log("khoa vat ly");
            rb.bodyType = RigidbodyType2D.Kinematic; // Đặt chế độ Kinematic để không bị ảnh hưởng bởi lực vật lý
        }
    }

}
