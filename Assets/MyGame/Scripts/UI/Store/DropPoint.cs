using UnityEngine;

public class DropPoint : MonoBehaviour
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
