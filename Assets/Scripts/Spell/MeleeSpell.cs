using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//베기
public class Slash : Spell
{
    List<Vector2> range = new List<Vector2>(){Vector2.left, Vector2.right, Vector2.up, Vector2.down, Vector2.left * 2, Vector2.right * 2, Vector2.up * 2, Vector2.down * 2};
    int damage;

    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> tilePositions = new List<Vector2>();
        foreach(Vector2 v in range){
            if(Gridlib.InRange(stage, stage.player.position + v)){
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
                e.GetDamage(damage);//damage 나중에 바꿀것
            }
        }
    }
}


//찌르기
public class Poke : Spell
{
    List<Vector2> range = new List<Vector2>(){Vector2.left, Vector2.right, Vector2.up, Vector2.down};
    int damage;
    
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> tilePositions = new List<Vector2>();
        foreach(Vector2 v in range){
            if(Gridlib.InRange(stage, stage.player.position + v)){
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
                e.GetDamage(damage);
            }
        }
    }
}

