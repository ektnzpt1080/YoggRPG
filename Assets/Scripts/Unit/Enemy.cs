using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public UnitData unitdata;
    public EnemyInfo info;
    [SerializeField] public int health;
    [SerializeField] public int strength;    
    [SerializeField] public Vector2 position;
    [SerializeField] public List<Vector2> attackRange;

    public void GetDamage( int damage ){
        health -= damage;
        if(health == 0){
            Debug.Log("enemy died");
            GameManager.Instance.BattleManager.RemoveEnemy(this);
            Destroy(this.gameObject);
        }
        else Debug.Log("enemy get damage");
    }

    public abstract void Move(Stage stage);
    public abstract void Attack(Stage stage);


}