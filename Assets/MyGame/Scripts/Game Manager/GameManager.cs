using DamageNumbersPro;
using System;
using System.Collections.Generic;
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
    public List<GameObject> itemsPrefab;
    public List<GameObject> shopItemPositions;
    public Camera worldCamera;
    public Transform storagePosition;  // Vị trí mà weapon sẽ rơi vào khi bị thay thế
    public GameObject[] bagAssignments = new GameObject[7];  // Mảng lưu trữ 7 bag
    public GameObject[] bags = new GameObject[7];
    public GameObject left;
    private WeaponController[] weaponControllers;
    public SpriteRenderer[] weaponAvatars = new SpriteRenderer[7];

    public Character leftChar;
    public Character rightChar;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        weaponControllers = left.GetComponents<WeaponController>();

        int index = 0;
        foreach (Transform child in left.transform)
        {
            if (child.name.StartsWith("Left Weapon"))
            {
                weaponAvatars[index] = child.GetComponent<SpriteRenderer>();
                index++;
            }
        }

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Roll();
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
            rightChar.hp = Math.Max(rightChar.hp - (int)damage, 0);
            rightChar.UpdateHp();
            if (rightChar.hp <= 0)
            {
                Instance.isStartGame = false;
                dialogWin.SetActive(true);
            }
        }
        else
        {
            leftChar.hp = Math.Max(leftChar.hp - (int)damage, 0);
            leftChar.UpdateHp();
            if (leftChar.hp <= 0)
            {
                Instance.isStartGame = false;
                dialogLose.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        Instance.isStartGame = true;
        Instance.shop.SetActive(false);
        Instance.rightCharacter.SetActive(true);
        Instance.rightCharacterUI.SetActive(true);
        HideShadow();
        ShowWeaponAvatar();
    }

    public void CloseDialog(GameObject dialog)
    {
        dialog.SetActive(false);
    }

    public void Roll()
    {
        for (int i = 0; i < shopItemPositions.Count; i++)
        {
            ClearChildObjects(shopItemPositions[i]);
            int index = UnityEngine.Random.Range(0, itemsPrefab.Count);
            GameObject newItem = Instantiate(itemsPrefab[index], shopItemPositions[i].transform);
        }
    }

    public void ClearChildObjects(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateCharacterStats(GameObject newWeapon, GameObject oldWeapon)
    {
        // Add new weapon stats
        UpdateAttributes(ItemManager.Instance.GetAttributes(newWeapon), true);

        // Remove old weapon stats
        UpdateAttributes(ItemManager.Instance.GetAttributes(oldWeapon), false);
    }

    private void UpdateAttributes(List<AttributeOption> attributes, bool isAdding)
    {
        foreach (var attribute in attributes)
        {
            int value = (int)attribute.value * (isAdding ? 1 : -1);

            switch (attribute.attribute)
            {
                case WeaponAttribute.DAMAGE:
                    leftChar.damage += value;
                    break;
                case WeaponAttribute.HP:
                    leftChar.hp += value;
                    //UpdateHp(); // Uncomment to update health bar when HP changes
                    break;
                case WeaponAttribute.MP:
                    leftChar.mp += value;
                    break;
                case WeaponAttribute.ATTACK_SPEED:
                    leftChar.attackSpeed += value;
                    break;
                case WeaponAttribute.CRIT:
                    leftChar.crit += value;
                    break;
                case WeaponAttribute.ARMOR:
                    leftChar.armor += value;
                    break;
                case WeaponAttribute.STR:
                    leftChar.str += value;
                    break;
                case WeaponAttribute.AGI:
                    leftChar.agi += value;
                    break;
                case WeaponAttribute.INT:
                    leftChar.intel += value;
                    break;
                case WeaponAttribute.DEX:
                    leftChar.dex += value;
                    break;
                case WeaponAttribute.LUCK:
                    leftChar.luck += value;
                    break;
                default:
                    Debug.LogWarning("Unknown attribute: " + attribute.attribute);
                    break;
            }
        }
    }

    public void UpdateShadow()
    {
        for (int i = 0; i < Instance.bagAssignments.Length; i++)
        {
            if (Instance.bagAssignments[i] == null)
            {
                Instance.bags[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
            }
            else
            {
                Instance.bags[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    public void HideShadow()
    {
        for (int i = 0; i < Instance.bags.Length; i++)
        {
            Instance.bags[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }

    public void UpdateUI(int slot, GameObject weapon)
    {
        Instance.bagAssignments[slot] = weapon;
        Instance.weaponControllers[slot].weapon = weapon;
        Instance.weaponAvatars[slot].sprite = weapon.GetComponent<SpriteRenderer>().sprite;

        if (weapon != null)
        {
            List<AttributeOption> attributes = ItemManager.Instance.GetAttributes(weapon);
            float attackSpeed = ItemManager.Instance.GetValue(attributes, WeaponAttribute.ATTACK_SPEED);
            Instance.weaponControllers[slot].attackSpeed = attackSpeed;
        }
    }

    public void ShowWeaponAvatar()
    {
        for (int i = 0; i < Instance.weaponAvatars.Length; i++)
        {
            if (Instance.bagAssignments[i] != null)
            {
                Instance.weaponAvatars[i].enabled = true;
            }
        }
    }
}
