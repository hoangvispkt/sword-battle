using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHoverPopup : MonoBehaviour
{
    public GameObject popup;        // Reference đến UI popup panel
    public TextMeshProUGUI titleText;          // Text field cho tên vũ khí
    public TextMeshProUGUI contentText;        // Text field cho thông số vũ khí
    public RectTransform canvasRectTransform; // Canvas RectTransform

    private RectTransform popupRectTransform;

    public static WeaponHoverPopup Instance { get; private set; }

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

    void Start()
    {
        popupRectTransform = popup.GetComponent<RectTransform>();
        popup.SetActive(false); // Ban đầu ẩn popup
    }

    void Update()
    {
        // Nếu popup đang được active thì điều chỉnh vị trí
        if (popup.activeSelf)
        {
            AdjustPopupPosition();
        }
    }

    // Gọi khi hover vào vũ khí
    public void OnMouseEnterWeapon(string weaponName, string weaponStatsHTML)
    {
        // Cập nhật nội dung
        titleText.text = weaponName;
        contentText.text = weaponStatsHTML;

        // Hiển thị popup
        popup.SetActive(true);

        // Cập nhật vị trí ban đầu của popup
        AdjustPopupPosition();
    }

    // Gọi khi rời khỏi vũ khí
    public void OnMouseExitWeapon()
    {
        popup.SetActive(false); // Ẩn popup khi rời chuột
    }

    // Điều chỉnh vị trí popup để đảm bảo nó không ra khỏi màn hình
    private void AdjustPopupPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        // Lấy kích thước của canvas và popup
        Vector2 canvasSize = canvasRectTransform.sizeDelta;
        Vector2 popupSize = popupRectTransform.sizeDelta;

        // Điều chỉnh vị trí popup dựa vào vị trí con trỏ và cạnh màn hình
        Vector2 adjustedPosition = mousePosition;

        // Kiểm tra nếu popup vượt ra ngoài phải màn hình
        if (mousePosition.x + popupSize.x > canvasSize.x)
        {
            float over = (mousePosition.x + popupSize.x) - canvasSize.x;
            adjustedPosition.x = Mathf.Max(canvasSize.x - over, canvasSize.x - popupSize.x/2);
        }

        // Kiểm tra nếu popup vượt ra ngoài trái màn hình
        if (mousePosition.x < popupSize.x)
        {
            adjustedPosition.x = popupSize.x;
        }

        // Kiểm tra nếu popup vượt ra ngoài trên màn hình
        if (mousePosition.y + popupSize.y > canvasSize.y)
        {
            float over = (mousePosition.y + popupSize.y) - canvasSize.y;
            adjustedPosition.y = canvasSize.y - over;
        }

        // Kiểm tra nếu popup vượt ra ngoài dưới màn hình
        if (mousePosition.y < popupSize.y)
        {
            adjustedPosition.y = popupSize.y;
        }

        // Cập nhật vị trí popup
        popupRectTransform.position = adjustedPosition;
    }
}
