using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int strength;    
    public Vector2 position;
    public List<Vector2> attackRange;

    public void GetDamage( int damage ){
        DamageText dt = Instantiate(GameManager.Instance.BattleManager.damageText, this.transform.position, Quaternion.identity);
        dt.Initialize("-" + damage, 0.5f);
        health -= damage;
        if(health <= 0){
            Debug.Log("enemy died");
            GameManager.Instance.BattleManager.RemoveEnemy(this);
            Destroy(this.gameObject);
        }
        else Debug.Log(this.name + " get damage : " + damage);
    }

    public void Init(EnemyInfo ei){
        health = ei.health;
        strength = ei.strength;
        GameManager.Instance.UIManager.HPBarInit(this);
        
    }
    
    public abstract void Move(Stage stage);
    public abstract void Attack(Stage stage);


}