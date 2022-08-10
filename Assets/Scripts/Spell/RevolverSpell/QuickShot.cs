using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickShot : RevolverSpell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        return Gridlib.WayStraightBlockable(stage, stage.player.position, false);
    }

    public override List<Vector2> YoggDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> result = new List<Vector2>();
        foreach( Vector2 v in PreDecision()){
            if( stage.EnemyTile().Contains(v)){
                result.Add(v);
            }
        }
        return result;
    }
    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        Vector2 nv = (stage.player.position - selectedPos).normalized;
        Vector2 damagePos = stage.player.position + nv;
        while(Gridlib.InRange(stage, damagePos)){
            Enemy damageEnemy = null;
            foreach(Enemy e in stage.enemies){
                if(e.position == damagePos) {
                    damageEnemy = e; 
                    break;
                }
            }
            if(damageEnemy != null){
                damageEnemy.GetDamage(GetValue());
                break;
            }
            damagePos += nv;
        }
        
        //카드 하나 랜덤으로 버리게 할 것
    }

    public override void Discarded(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> vl = YoggDecision();
        Vector2 nv;
        if(vl.Count == 0){
            nv = Vector2.right;    
        }
        else {
            int index = Random.Range(0, vl.Count);
            nv = (vl[index] - stage.player.position).normalized;
        }
        Vector2 damagePos = stage.player.position + nv;
        while(Gridlib.InRange(stage, damagePos)){
            Enemy damageEnemy = null;
            foreach(Enemy e in stage.enemies){
                if(e.position == damagePos) {
                    damageEnemy = e; 
                    break;
                }
            }
            if(damageEnemy != null){
                damageEnemy.GetDamage(GetValue());
                break;
            }
            damagePos += nv;
        }
    }

}
