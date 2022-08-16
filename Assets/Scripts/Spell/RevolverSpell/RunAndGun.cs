using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/RunAndGun")]
public class RunAndGun : RevolverSpell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> result = new List<Vector2>();
        List<Vector2> candidate = Gridlib.WayStraightBlockable(stage, stage.player.position, false, 2);
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
    public override void Discarded(){
        GameManager.Instance.CardManager.DrawCard();
    }
}
