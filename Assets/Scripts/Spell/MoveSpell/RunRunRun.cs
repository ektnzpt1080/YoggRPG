using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/RunRunRun")]
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
    public override void GetSpellInfo(){
        spellinfo = GameManager.Instance.CardManager.getSpellInfo(202);
    }
}