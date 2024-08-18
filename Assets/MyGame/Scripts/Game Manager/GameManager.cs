using DamageNumbersPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DamageNumber numberPrefab;
    public bool isStartGame = false;
    public GameObject dialogWin;
    public GameObject dialogLose;
    public GameObject shop;
    public GameObject rightCharacter;
    public GameObject rightCharacterUI;

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
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position, Random.Range(1, 100));
    }

    public void StartGame()
    {
        Instance.isStartGame = true;
        Instance.shop.SetActive(false);
        Instance.rightCharacter.SetActive(true);
        Instance.rightCharacterUI.SetActive(true);
    }

    public void CloseDialog(GameObject dialog)
    {
        dialog.SetActive(false);
    }
}
