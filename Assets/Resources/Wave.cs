using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Wave", menuName = "ScriptableObjects/Wave", order = 1)]
public class Wave : ScriptableObject
{
    public int spawnAmount;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
}
