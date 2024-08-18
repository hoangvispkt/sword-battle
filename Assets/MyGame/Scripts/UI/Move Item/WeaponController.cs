using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject weapon;
    public Transform spawnPoint;
    public Transform pointUp;
    public Transform pointDown;
    public float attackSpeed;

    private bool isPointUp = true;
    private float lastAttackTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isStartGame) return;
        if (CanAttack())
        {
            GameObject newWeapon = Instantiate(this.weapon, spawnPoint.position, spawnPoint.rotation);
            if (isPointUp)
            {
                newWeapon.AddComponent<Follow>().setSpeed(0.5f).setRoute(pointUp).start();
                isPointUp = false;
            }
            else
            {
                newWeapon.AddComponent<Follow>().setSpeed(0.5f).setRoute(pointDown).start();
                isPointUp = true;
            }
            lastAttackTime = Time.time;
        }
    }

    private bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackSpeed;
    }
}
