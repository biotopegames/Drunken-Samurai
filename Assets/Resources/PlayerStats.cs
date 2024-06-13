using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player Stats", menuName = "ScriptableObjects/PlayerStats", order = 2)]
public class PlayerStats : ScriptableObject
{
    public float dashSpeed = 1f;
    public float dashCooldown = 2f; // Seconds
    public float moveSpeed = 5f;
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int damage;
    public int armor;
    public int level;
    public int currentExp = 0;
    public int expNeededToLevelUp = 0;


}

