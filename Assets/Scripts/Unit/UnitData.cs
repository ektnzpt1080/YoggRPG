using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public string Name;
    public Sprite sprite;
    public int health;
    public int strength;

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    //player
    public int playerHP;
    public int playerStrength;
    public int playerIntelligence;
    public int playerMana;
    
    public EnemyInfo[] Enemies;
}