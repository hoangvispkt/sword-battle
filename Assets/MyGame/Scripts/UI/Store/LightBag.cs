using UnityEngine;

public class LightBag : MonoBehaviour
{
    public Color highlightColor = Color.yellow; // Màu sẽ thay đổi khi làm nổi bật
    public SpriteRenderer spriteRenderer; // Tham chiếu đến SpriteRenderer của túi
    private Color originalColor;

    private void Start()
    {
        // Lưu màu ban đầu của túi
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("weapo")) // Giả sử vật phẩm có tag là "Item"
        {
            Debug.Log("nhan");
            HighlightBag(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("weapo"))
        {
            Debug.Log("thoat");
            HighlightBag(false);
        }
    }

    private void HighlightBag(bool highlight)
    {
        // Thay đổi màu của túi khi làm nổi bật
        if (highlight)
        {
            spriteRenderer.color = highlightColor;
        }
        else
        {
            spriteRenderer.color = originalColor;
        }
    }
}
