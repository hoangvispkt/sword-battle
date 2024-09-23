using DamageNumbersPro;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DamageNumber numberPrefabMesh;
    public DamageNumber numberPrefabGUI;
    public bool isStartGame = false;
    public GameObject dialogWin;
    public GameObject dialogLose;
    public GameObject shop;
    public GameObject rightCharacter;
    public GameObject rightCharacterUI;
    public List<GameObject> itemsPrefab;
    public List<GameObject> shopItemPositions;
    public List<TextMeshProUGUI> shopItemPrices;
    public Camera worldCamera;
    public Transform storagePosition;  // Vị trí mà weapon sẽ rơi vào khi bị thay thế
    public GameObject[] bagAssignments = new GameObject[7];  // Mảng lưu trữ 7 bag
    public GameObject[] shadows = new GameObject[7];
    public GameObject left;
    private WeaponController[] weaponControllers;
    public Transform items;
    public GameObject rollButton;
    public TextMeshProUGUI goldUI;
    public TextMeshProUGUI lifeUI;
    public TextMeshProUGUI winUI;
    public TextMeshProUGUI loseUI;
    public TextMeshProUGUI roundUI;

    public Character leftChar;
    public Character rightChar;

    public static GameManager Instance { get; private set; }
    private bool isFirstRoll = true;
     
    private void Awake()
    {
        weaponControllers = left.GetComponents<WeaponController>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Roll();
    }

    public void HandleAttack(GameObject weapon, Transform transform, bool isLeftChar)
    {
        List<AttributeOption> attributes = ItemManager.Instance.GetAttributes(weapon);
        float damage = ItemManager.Instance.GetValue(attributes, WeaponAttribute.DAMAGE);
        DamageNumber damageNumber = numberPrefabMesh.Spawn(transform.position, damage);

        float mpRequired = ItemManager.Instance.GetValue(attributes, WeaponAttribute.MP_REQUIRED);
        this.UpdateCharacterStats(isLeftChar, damage, mpRequired);
    }

    private void UpdateCharacterStats(bool isLeftChar, float damage, float mpRequired)
    {
        if (!Instance.isStartGame) return;
        if (isLeftChar)
        {
            rightChar.hp = Math.Max(rightChar.hp - (int)damage, 0);
            if (rightChar.hp <= 0)
            {
                Instance.isStartGame = false;
                dialogWin.SetActive(true);
            }
            leftChar.mp -= (int)mpRequired;
            rightChar.UpdateHp();
            leftChar.UpdateMp();
        }
        else
        {
            leftChar.hp = Math.Max(leftChar.hp - (int)damage, 0);
            if (leftChar.hp <= 0)
            {
                Instance.isStartGame = false;
                dialogLose.SetActive(true);
            }
            rightChar.mp -= (int)mpRequired;
            leftChar.UpdateHp();
            rightChar.UpdateMp();
        }
    }

    public void StartGame()
    {
        Instance.isStartGame = true;
        Instance.shop.SetActive(false);
        Instance.rightCharacter.SetActive(true);
        Instance.rightCharacterUI.SetActive(true);
        Instance.rollButton.SetActive(false);
        HideShadow();
        Instance.leftChar.round++;
        StartCoroutine(ManaRecovery());
    }

    public void CloseDialogWin()
    {
        dialogWin.SetActive(false);
        Instance.isStartGame = false;
        Instance.shop.SetActive(true);
        Instance.rightCharacter.SetActive(false);
        Instance.rightCharacterUI.SetActive(false);
        Instance.rollButton.SetActive(true);
        UpdateShadow();
        Instance.leftChar.gold += 20;
        Instance.leftChar.win++;
        Instance.leftChar.maxHp += 20;
        Instance.leftChar.hp = Instance.leftChar.maxHp;
        Instance.leftChar.mp = Instance.leftChar.maxMp;
        Instance.leftChar.attackSpeed = Instance.leftChar.maxAttackSpeed;
        Instance.leftChar.armor = Instance.leftChar.maxArmor;
        UpdateUI();
        Roll();
    }

    public void CloseDialogLose()
    {
        dialogLose.SetActive(false);
        Instance.isStartGame = false;
        Instance.shop.SetActive(true);
        Instance.rightCharacter.SetActive(false);
        Instance.rightCharacterUI.SetActive(false);
        Instance.rollButton.SetActive(true);
        UpdateShadow();
        Instance.leftChar.gold += 20;
        Instance.leftChar.lose++;
        Instance.leftChar.maxHp += 20;
        Instance.leftChar.hp = Instance.leftChar.maxHp;
        Instance.leftChar.mp = Instance.leftChar.maxMp;
        Instance.leftChar.attackSpeed = Instance.leftChar.maxAttackSpeed;
        Instance.leftChar.armor = Instance.leftChar.maxArmor;
        Instance.leftChar.life--;
        if (Instance.leftChar.life <= 0)
        {
            Debug.Log("End Game");
            ResetGame();
        }
        UpdateUI();
        Roll();
    }

    public void ResetGame()
    {

    }

    public void Roll()
    {
        if (Instance.leftChar.gold >= 1)
        {
            for (int i = 0; i < shopItemPositions.Count; i++)
            {
                ClearChildObjects(shopItemPositions[i]);
                int index = UnityEngine.Random.Range(0, itemsPrefab.Count);
                GameObject newItem = Instantiate(itemsPrefab[index], shopItemPositions[i].transform);
                List<AttributeOption> attributes = ItemManager.Instance.GetAttributes(newItem);
                int price = (int)ItemManager.Instance.GetValue(attributes, WeaponAttribute.PRICE);
                Instance.shopItemPrices[i].text = price.ToString();
            }
            if (!isFirstRoll)
            {
                Instance.leftChar.gold--;
            }
            isFirstRoll = false;
            UpdateUI();
        }
        else
        {
            numberPrefabGUI.SpawnGUI(Instance.rollButton.GetComponent<RectTransform>(), Vector2.zero, "Out of gold!");
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
                    leftChar.maxHp += value;
                    //UpdateHp(); // Uncomment to update health bar when HP changes
                    break;
                case WeaponAttribute.MP:
                    leftChar.mp += value;
                    leftChar.maxHp += value;
                    break;
                case WeaponAttribute.ATTACK_SPEED:
                    leftChar.attackSpeed += value;
                    leftChar.maxAttackSpeed += value;
                    break;
                case WeaponAttribute.CRIT:
                    leftChar.crit += value;
                    break;
                case WeaponAttribute.ARMOR:
                    leftChar.armor += value;
                    leftChar.maxArmor += value;
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
                Instance.shadows[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
            }
            else
            {
                Instance.shadows[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    public void HideShadow()
    {
        for (int i = 0; i < Instance.shadows.Length; i++)
        {
            Instance.shadows[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }

    public void UpdateWeaponUI(int slot, GameObject weapon)
    {
        Instance.bagAssignments[slot] = weapon;
        Instance.weaponControllers[slot].weapon = weapon;
        if (weapon != null)
        {
            List<AttributeOption> attributes = ItemManager.Instance.GetAttributes(weapon);
            float attackSpeed = ItemManager.Instance.GetValue(attributes, WeaponAttribute.ATTACK_SPEED);
            Instance.weaponControllers[slot].attackSpeed = attackSpeed;
        }
    }

    public void UpdateUI()
    {
        Instance.goldUI.text = leftChar.gold.ToString();
        Instance.lifeUI.text = leftChar.life.ToString();
        Instance.winUI.text = leftChar.win.ToString();
        Instance.loseUI.text = leftChar.lose.ToString();
        Instance.roundUI.text = leftChar.round.ToString();
        leftChar.UpdateHp();
        leftChar.UpdateMp();
    }

    private IEnumerator ManaRecovery()
    {
        while (Instance.isStartGame)
        {
            yield return new WaitForSeconds(1f);
            leftChar.mp = Mathf.Max(leftChar.mp + leftChar.mpRecovery, leftChar.maxMp);
            leftChar.UpdateMp();
        }
    }
}
