using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//스텝
public class Step : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> result = new List<Vector2>();
        List<Vector2> candidate = Gridlib.Circle(stage.player.position);
        foreach(Vector2 c in candidate){
            if(!stage.AssignedTile().Contains(c)){
                result.Add(c);
            }
        }
        return result;
    }


    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        stage.player.Move(stage.tiles[selectedPos]);
    }
}

//달려!
public class RunRunRun : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> result = new List<Vector2>();
        List<Vector2> candidate = Gridlib.WayStraight(stage.player.position, false, 3);
        foreach(Vector2 c in candidate){
            if(!stage.AssignedTile().Contains(c)){
                result.Add(c);
            }
        }
        return result;
    }


    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        stage.player.Move(stage.tiles[selectedPos]);
    }
}