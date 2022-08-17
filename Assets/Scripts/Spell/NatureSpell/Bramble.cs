using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/Bramble")]
public class Bramble : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        return Gridlib.Circle(stage, stage.player.position, 2);
    }
    public override List<Vector2> YoggDecision(){
        return PreDecision();
    }
    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        stage.player.spike += 5;
        Vector2 pp = stage.player.position;
        List<Vector2> attackRange = Gridlib.Circle(stage, stage.player.position, 2);
        foreach(Vector2 damagePos in attackRange){
            Enemy damageEnemy = null;
            foreach(Enemy e in stage.enemies){
                if(e.position == damagePos) {
                    damageEnemy = e; 
                    break;
                }
            }
            if(damageEnemy != null){
                damageEnemy.GetDamage(GetValue() * stage.player.spike);
            }
        }        
    }
    

}
