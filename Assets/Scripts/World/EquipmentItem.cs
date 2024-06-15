using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Items/Equipment Item")]
public class EquipmentItem : Item
{
    public EquipmentType equipmentType;

    public int damageStat;
    public int defenseStat;
    public int healthStat;
    public float moveSpeedStat;
}

public enum EquipmentType
{
    Medallion,
    Weapon
}
