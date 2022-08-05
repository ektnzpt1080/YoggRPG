using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyData unitinfo;
    [SerializeField] public int health;
    [SerializeField] public int strength;    
    [SerializeField] public Vector2 position;
    [SerializeField] public List<Vector2> attackRange;

    public void GetDamage( int damage ){
        DamageText dt = Instantiate(GameManager.Instance.BattleManager.damageText, this.transform.position, Quaternion.identity);
        dt.Initialize("-" + damage, 0.5f);
        health -= damage;
        if(health <= 0){
            Debug.Log("enemy died");
            GameManager.Instance.BattleManager.RemoveEnemy(this);
            Destroy(this.gameObject);
        }
        else Debug.Log("enemy get damage");
    }

    public abstract void Init(EnemyInfo enemyInfo);
    public abstract void Move(Stage stage);
    public abstract void Attack(Stage stage);


}