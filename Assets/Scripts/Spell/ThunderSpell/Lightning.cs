using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/Lightning")]
public class Lightning : Spell
{
    public override List<Vector2> PreDecision(){
        return null;
    }
    public override List<Vector2> YoggDecision(){
        return null;
    }

    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        int index = Random.Range(0, stage.EnemyTile().Count);
        List<Vector2> damagePos = Gridlib.Circle(stage, stage.EnemyTile()[index]);
        damagePos.Add(stage.EnemyTile()[index]);
        for(int i = stage.enemies.Count - 1 ; i >= 0 ; i--){
            if(damagePos.Contains(stage.enemies[i].position)){
                stage.enemies[i].GetDamage(GetValue());
            }
        }
    }
}
