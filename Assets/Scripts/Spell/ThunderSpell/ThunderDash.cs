using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/ThunderDash")]
public class ThunderDash : Spell
{
    protected int thunderCount;
    public override List<Vector2> PreDecision(){
        return null;
    }
    public override List<Vector2> YoggDecision(){
        return null;
    }

    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        Vector2 pp = stage.player.position;
        List<Vector2> way4 = new List<Vector2>() {Vector2.left, Vector2.right, Vector2.up, Vector2.down};
        List<Vector2> candidate = new List<Vector2>();
        foreach(Vector2 v in way4){
            if(Gridlib.InRange(stage, pp + v * 3)){
                candidate.Add(pp + v*3);
            }
        }
        if(candidate.Count != 0 ) {
            int i = Random.Range(0, candidate.Count);
            Vector2 nv = (candidate[i] - pp).normalized;
            for(int j = 1 ; j <= 3; j++){
                Enemy damageEnemy = null;
                Vector2 damagePos = pp + nv * j;
                foreach(Enemy e in stage.enemies){
                    if(e.position == damagePos) {
                        damageEnemy = e;
                        break;
                    }
                }   
                if(damageEnemy != null){
                    damageEnemy.GetDamage(GetValue());
                } 
            }
            stage.player.Move(stage.tiles[candidate[i]]);
        }
    }
}
