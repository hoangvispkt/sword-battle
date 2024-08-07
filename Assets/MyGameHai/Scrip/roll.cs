using UnityEngine;

public class roll : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("point"))
        {
            Debug.Log("+1 roll");
        }
    }
}
