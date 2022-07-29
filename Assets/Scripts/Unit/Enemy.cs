using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [SerializeField] public int health;
    [SerializeField] public Vector2 position;

    public void GetDamage( int damage ){
        health -= damage;
        if(health == 0){
            Debug.Log("enemy died");
            GameManager.Instance.BattleManager.RemoveEnemy(this);
            Destroy(this.gameObject);
        }
        else Debug.Log("enemy get damage");
    }
    public abstract void Behaviour(Stage stage);

}