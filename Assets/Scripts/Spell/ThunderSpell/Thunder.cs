using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/Thunder")]
public class Thunder : Spell
{
    protected int thunderCount;
    public override List<Vector2> PreDecision(){
        return null;
    }
    public override List<Vector2> YoggDecision(){
        return null;
    }

    public override void Decision(Vector2 selectedPos){
        thunderCount = 4;
        RandomTargeting(thunderCount);
    }

    public void RandomTargeting(int _Count){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        List<Vector2> targets = new List<Vector2>();
        bool repeatable = (Mathf.FloorToInt(stage.size.x) - 1) * (Mathf.FloorToInt(stage.size.y) - 1) <= _Count;

        while(targets.Count < _Count) {
            int x = Random.Range(0, Mathf.FloorToInt(stage.size.x) - 1);
            int y = Random.Range(0, Mathf.FloorToInt(stage.size.y) - 1);
            if(!targets.Contains(new Vector2(x, y)) || repeatable) targets.Add(new Vector2(x, y));
        }

        foreach(Vector2 v in targets){
            Debug.Log(v.ToString());
            List<Vector2> damagePos = new List<Vector2>() {v, v+Vector2.right, v+Vector2.up, v+Vector2.up+Vector2.right };
            for(int i = stage.enemies.Count - 1 ; i >= 0 ; i--){
                if(damagePos.Contains(stage.enemies[i].position)){
                    stage.enemies[i].GetDamage(GetValue());
                }
            }
                
        }
    }

}
