using UnityEngine;

public class roll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)//dùng khi muốn 2 vật thể xuyên qua nhau ĐK : phải bật istrigger
    {
        if (collision.gameObject.tag == "rope")
        {
            Debug.Log("+1 roll");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)//dùng khi không muốn 2 vật thể xuyên qua nhau khong co DK
    {
        if (collision.gameObject.tag == "rope")
        {
            Debug.Log("+1 suc sac");
        }
    }
}
