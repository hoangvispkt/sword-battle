using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int damage;
    public int hp;
    public int maxHp;
    public int mp;
    public int mpRecovery;
    public int maxMp;
    public int attackSpeed;
    public int maxAttackSpeed;
    public int crit;
    public int armor;
    public int maxArmor;
    public int str;
    public int agi;
    public int intel;
    public int dex;
    public int luck;
    public int gold;
    public int life;
    public int win;
    public int lose;
    public int round;

    public GameObject hpBar;
    public TextMeshProUGUI hpText;

    public GameObject mpBar;
    public TextMeshProUGUI mpText;

    public void UpdateHp()
    {
        float fillAmount = (float)hp / maxHp;
        hpBar.GetComponent<Image>().fillAmount = fillAmount;
        hpText.text = hp + "/" + maxHp;
    }

    public void UpdateMp()
    {
        float fillAmount = (float)mp / maxMp;
        mpBar.GetComponent<Image>().fillAmount = fillAmount;
        mpText.text = mp + "/" + maxMp;
    }
}
