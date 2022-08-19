using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCard : MonoBehaviour
{
    [SerializeField] TextMeshPro nameText;
    [SerializeField] TextMeshPro attackText;
    [SerializeField] TextMeshPro hpText;
    [SerializeField] SpriteRenderer EnemySpriteRenderer;

    public void InitEnemyCard(Enemy enemy){
        nameText.text = enemy.enemyinfo.name;
        hpText.text = enemy.health + "/" + enemy.maxHealth;
    }
}