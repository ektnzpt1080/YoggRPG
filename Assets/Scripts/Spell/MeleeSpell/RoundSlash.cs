using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/RoundSlash")]
//회전베기 103
public class RoundSlash : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        return Gridlib.Circle(stage, stage.player.position);
    }
    public override List<Vector2> YoggDecision(){
        return PreDecision();
    }

    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        foreach (Vector2 v in Gridlib.Circle(stage, stage.player.position)){
            for(int i = stage.enemies.Count - 1 ; i >= 0; i--){
                if(stage.enemies[i].position == v){
                    stage.enemies[i].GetDamage(GetValue());
                }
            }
        }
    }
}