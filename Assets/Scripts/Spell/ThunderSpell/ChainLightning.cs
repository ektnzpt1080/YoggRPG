using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/ChainLightning")]
public class ChainLightning : Spell
{
    public override List<Vector2> PreDecision(){
        return null;
    }
    public override List<Vector2> YoggDecision(){
        return null;
    }

    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        int index = Random.Range(0, stage.enemies.Count);
        Enemy damageEnemy = stage.enemies[index];
        damageEnemy.GetDamage(GetValue());
        
        bool reattack = Random.Range(0,2) == 0;
        while(stage.enemies.Count > 1 && reattack){
            Debug.Log("Reattack");
            Enemy reattackEnemy = damageEnemy;
            while (reattackEnemy == damageEnemy){
                index = Random.Range(0, stage.enemies.Count);
                reattackEnemy = stage.enemies[index];
            }
            reattackEnemy.GetDamage(GetValue());
            damageEnemy = reattackEnemy;
            reattack = Random.Range(0,2) == 0;
        }
    }

    public void DoDamage(Enemy _e){        
        _e.GetDamage(GetValue());
    }

}
