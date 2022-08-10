using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/WoodShield")]
//나무장벽 301
public class WoodShield : Spell
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
    }

}