using DamageNumbersPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DamageNumber numberPrefab;
    public RectTransform rectParent;
    public Transform target;

    public static GameManager Instance { get; private set; }

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

    public void HandleAttack(GameObject weapon, Transform transform)
    {
        Debug.Log(weapon);
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position, Random.Range(1, 100));
    }
}
