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


    public int TotalHp = 0;
    public int TotalDamage = 0;
    public int TotalMp = 0;
    public int TotalCrit = 0;

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
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position, Random.Range(1, TotalDamage));
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

    public void UpdateTotalAttributes(int hp, int damage, int mp, int crit)
    {
        TotalHp += hp;
        TotalDamage += damage;
        TotalMp += mp;
        TotalCrit += crit;

        Debug.Log($"Tổng sau khi cập : HP = {TotalHp}, Damage = {TotalDamage}, MP = {TotalMp}, Crit = {TotalCrit}");
    }
}
