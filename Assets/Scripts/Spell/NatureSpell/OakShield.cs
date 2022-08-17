using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/OakShield")]
public class OakShield : Spell
{
    public override List<Vector2> PreDecision(){
        return null;
    }
    public override List<Vector2> YoggDecision(){
        return PreDecision();
    }
    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        stage.player.shield += GetValue();
        stage.player.spike += 7;
    }

}
