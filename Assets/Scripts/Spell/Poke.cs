using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poke : Spell
{
    List<Vector2> range = new List<Vector2>(){Vector2.left, Vector2.right, Vector2.up, Vector2.down, Vector2.left * 2, Vector2.right * 2, Vector2.up * 2, Vector2.down * 2};

    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> tilePositions = new List<Vector2>();
        foreach(Vector2 v in range){
            if(Gridlib.InRange(stage.size, stage.player.position + v)){
                stage.tiles[stage.player.position + v].Highlight();
                foreach (Enemy e in stage.enemies){
                    if(e.position == stage.player.position + v){
                        tilePositions.Add(stage.player.position + v);
                    }
                }
            }
        }
        return tilePositions;
    }

    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        foreach (Enemy e in stage.enemies){
            if(e.position == selectedPos){
                e.GetDamage(5);//damage 나중에 바꿀것
            }
        }
    }

}
