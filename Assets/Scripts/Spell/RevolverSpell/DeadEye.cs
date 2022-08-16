using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/DeadEye")]
public class DeadEye : RevolverSpell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        return Gridlib.WayStraight(stage, stage.player.position, false);
    }

    public override List<Vector2> YoggDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        List<Vector2> result = new List<Vector2>();
        foreach( Vector2 v in PreDecision()){
            if( stage.EnemyTile().Contains(v)){
                result.Add(v);
            }
        }
        return result;
    }
    public override void Decision(Vector2 selectedPos){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        Vector2 nv = (selectedPos - stage.player.position).normalized;
        Vector2 damagePos = stage.player.position + nv;
        while(Gridlib.InRange(stage, damagePos)){
            Enemy damageEnemy = null;
            foreach(Enemy e in stage.enemies){
                if(e.position == damagePos) {
                    damageEnemy = e; 
                    break;
                }
            }
            if(damageEnemy != null){
                damageEnemy.GetDamage(GetValue());
            }
            damagePos += nv;
        }
    }

    public override void Discarded(){
        List<SpellInfo> grave = GameManager.Instance.CardManager.cardGrave;
        SpellInfo target = grave[grave.Count - 1];
        if ( target.spell.GetType() == typeof(DeadEye) ) {
            Debug.Log("DeadEye Activate");
            target.cost -= 1;
            GameManager.Instance.CardManager.MakeCard(target);
            grave.Remove(target);
        }
    }

}
