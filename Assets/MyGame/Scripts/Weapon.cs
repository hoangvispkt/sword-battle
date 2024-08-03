using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public GameObject weapon;
    public Transform spawnPoint;
    public Transform pointUp;
    public Transform pointDown;
    public float attackSpeed;
    public bool isPointUp = true;
    public float lastAttackTime = 0f;
}
