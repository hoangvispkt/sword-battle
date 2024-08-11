using UnityEngine;

public class droppoint : MonoBehaviour
{
    public Transform createPoint;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "weapo")
        {
            collision.gameObject.transform.position = createPoint.position;
        }
    }
}
