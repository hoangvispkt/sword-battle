using UnityEngine;

public class _drangAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private bool shouldCheckCollision = false; // Cờ để kiểm tra va chạm
    private Vector3 offset;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure the object has a Rigidbody2D
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        shouldCheckCollision = false; // Không kiểm tra va chạm khi đang kéo
    }

    private void OnMouseUp()
    {
        isDragging = false;
        shouldCheckCollision = true; // Cho phép kiểm tra va chạm khi nhả chuột
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 mousePos = GetMouseWorldPos();
        // Chỉ kiểm tra va chạm nếu cờ shouldCheckCollision là true
        if (shouldCheckCollision && collision.tag == "sword")
        {
            Debug.Log("Nhận được rồi");
            // Đặt vị trí của đối tượng tại vị trí của scabbardRect
            //collision.transform.position = scabbardRect.position;
            transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);
            // Đảm bảo đối tượng đứng yên tại vị trí mới
            rb.velocity = Vector2.zero;       // Ngừng chuyển động
            rb.angularVelocity = 0f;          // Ngừng xoay
            Debug.Log("Khóa vật lý");
            rb.bodyType = RigidbodyType2D.Kinematic; // Đặt chế độ Kinematic để không bị ảnh hưởng bởi lực vật lý

            // Reset cờ sau khi va chạm đã được xử lý
            shouldCheckCollision = false;
        }
    }

}
