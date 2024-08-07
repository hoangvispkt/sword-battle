using UnityEngine;

public class plus_index : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "plus")
        {
            Debug.Log("+1 damage");
        }
    }
}
