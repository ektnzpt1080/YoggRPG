using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//스텝 201
public class Step : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> result = new List<Vector2>();
        List<Vector2> candidate = Gridlib.WayStraight(stage, stage.player.position, false, 1);
        foreach(Vector2 c in candidate){
            if(!stage.AssignedTile().Contains(c)){
                result.Add(c);
            }
        }
        return result;
    }
    public override List<Vector2> YoggDecision(){
        return PreDecision();
    }
    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        stage.player.Move(stage.tiles[selectedPos]);
    }
    public override SpellInfo GetSpellInfo(){
        return GameManager.Instance.CardManager.getSpellInfo(201);
    }
}

//달려! 202
public class RunRunRun : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> result = new List<Vector2>();
        List<Vector2> candidate = Gridlib.WayStraightBlockable(stage, stage.player.position, true, 3);
        foreach(Vector2 c in candidate){
            if(!stage.AssignedTile().Contains(c)){
                result.Add(c);//바꿔야됨, enemy에 막히도록
            }
        }
        return result;
    }
    public override List<Vector2> YoggDecision(){
        return PreDecision();
    }
    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        stage.player.Move(stage.tiles[selectedPos]);
    }
    public override SpellInfo GetSpellInfo(){
        return GameManager.Instance.CardManager.getSpellInfo(202);
    }
}