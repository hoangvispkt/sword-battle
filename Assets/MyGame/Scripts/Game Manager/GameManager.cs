using DamageNumbersPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public DamageNumber numberPrefab;
    public bool isStartGame = false;
    public GameObject dialogWin;
    public GameObject dialogLose;
    public GameObject shop;
    public GameObject rightCharacter;
    public GameObject rightCharacterUI;

    public Character leftChar;
    public Character rightChar;

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

    public void HandleAttack(GameObject weapon, Transform transform, bool isLeftChar)
    {
        List<AttributeOption> attributes = ItemManager.Instance.GetAttributes(weapon);
        float damage = ItemManager.Instance.GetValue(attributes, WeaponAttribute.DAMAGE);
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position, damage);

        this.UpdateCharacterStats(isLeftChar, damage);
    }

    private void UpdateCharacterStats(bool isLeftChar, float damage)
    {
        if (!Instance.isStartGame) return;
        if (isLeftChar)
        {
            leftChar.hp = Math.Max(leftChar.hp - (int)damage, 0);
            leftChar.UpdateHp();
            if (leftChar.hp <= 0)
            {
                Instance.isStartGame = false;
                dialogLose.SetActive(true);
            }
        }
        else
        {
            rightChar.hp = Math.Max(rightChar.hp - (int)damage, 0);
            rightChar.UpdateHp();
            if (rightChar.hp <= 0)
            {
                Instance.isStartGame = false;
                dialogWin.SetActive(true);
            }
        }
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
