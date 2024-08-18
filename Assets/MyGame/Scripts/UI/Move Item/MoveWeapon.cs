using System.Collections;
using UnityEngine;

public class MoveWeapon : MonoBehaviour
{
    public Transform target; // Đối tượng mục tiêu
    public float height = 2f; // Độ cao của đường cung

    private Vector3 startPosition; // Vị trí ban đầu của thanh kiếm
    private Vector3 endPosition; // Vị trí mục tiêu

    void Start()
    {
        // Lưu lại vị trí ban đầu và mục tiêu
        startPosition = transform.position;
        endPosition = target.position;

        // Bắt đầu chuyển động khi khởi tạo
        StartCoroutine(MoveSword());
    }

    IEnumerator MoveSword()
    {
        float duration = 2f; // Thời gian di chuyển
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Tính toán vị trí hiện tại dựa trên thời gian
            float t = elapsedTime / duration;
            Vector3 currentPos = CalculateBezierCurve(startPosition, endPosition, height, t);
            transform.position = currentPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đến đích, có thể thực hiện thêm hành động khác ở đây

    }

    Vector3 CalculateBezierCurve(Vector3 start, Vector3 end, float height, float t)
    {
        // Tính toán điểm trên đường cung Bézier với điều chỉnh độ cong
        float curveHeight = height * (1f - Mathf.Cos(t * Mathf.PI)); // Điều chỉnh độ cong ở đầu và đuôi

        float x = Mathf.Lerp(start.x, end.x, t);
        float y = Mathf.Lerp(start.y, end.y, t) + curveHeight;
        float z = Mathf.Lerp(start.z, end.z, t);
        return new Vector3(x, y, z);
    }
}
