using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//베기 101
public class Slash : Spell
{
    List<Vector2> range = new List<Vector2>(){Vector2.left, Vector2.right, Vector2.up, Vector2.down};

    public void Awake(){
        number = 101;
    }
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
                e.GetDamage(GetValue());//damage 나중에 바꿀것
            }
        }
    }
    public override SpellInfo GetSpellInfo(){
        return GameManager.Instance.CardManager.getSpellInfo(101);
    }
}


//찌르기 102
public class Poke : Spell
{
    List<Vector2> range = new List<Vector2>(){Vector2.left, Vector2.right, Vector2.up, Vector2.down, Vector2.left * 2, Vector2.right * 2, Vector2.up * 2, Vector2.down * 2};
    
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
                e.GetDamage(GetValue());
            }
        }
    }
    public override SpellInfo GetSpellInfo(){
        return GameManager.Instance.CardManager.getSpellInfo(102);
    }
}

//회전베기 103
public class RoundSlash : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        return Gridlib.Circle(stage, stage.player.position);
    }

    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        foreach (Vector2 v in Gridlib.Circle(stage, stage.player.position)){
            foreach(Enemy e in stage.enemies){
                if(e.position == v) {
                    e.GetDamage(GetValue());
                }
            }
        }
    }
    public override SpellInfo GetSpellInfo(){
        return GameManager.Instance.CardManager.getSpellInfo(103);
    }
}

//방패막기 104
public class ShieldUp : Spell
{
    public override List<Vector2> PreDecision(){
        return null;
    }
    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();        
        stage.player.shield += GetValue();
    }
    public override SpellInfo GetSpellInfo(){
        return GameManager.Instance.CardManager.getSpellInfo(104);
    }
}
