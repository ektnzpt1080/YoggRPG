using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Vector2 position { get; set;}
    [SerializeField] public int maxHealth { get; set;}
    [SerializeField] public int health { get; set;}
    [SerializeField] public int strength { get; set;}
    [SerializeField] public int intelligence { get; set;}
    [SerializeField] public int shield { get; set;}
    
    public void GetDamage(int d){
        string s;
        if(shield == 0){
            health -= d;
            s = "-" + d;
        }
        else if(shield >= d){
            shield -= d;
            s = "(-" + d + ")";
        }
        else {
            s = "(-"+ shield +")\n-" + d; 
            health -= d - shield;
            shield = 0;
        }

        DamageText dt = Instantiate(GameManager.Instance.BattleManager.damageText, this.transform.position, Quaternion.identity);
        dt.Initialize(s, 0.5f);

        Debug.Log("Player Get Damaged : -" + d);
        if(health <= 0 ){
            health = 0;
            GameManager.Instance.BattleManager.EndStagePlayerDead();
        }


    }

    public void Move(Tile tile){
        position = tile.position;
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, transform.position.z);
    }

    public void Initialize(PlayerData pd){
        maxHealth = pd.health;
        health = pd.health;
        strength = pd.strength;
        intelligence = pd.intelligence;
        shield = 0;
    }

    public void Initialize(Player p){
        maxHealth = p.maxHealth;
        health = p.health;
        strength = p.strength;
        intelligence = p.intelligence;
        shield = 0;
    }

    public void SynchroHP(Player p){
        health = p.health;
    }

}
