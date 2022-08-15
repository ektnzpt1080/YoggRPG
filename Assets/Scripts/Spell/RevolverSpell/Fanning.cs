using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/Fanning")]
public class Fanning : Spell
{
    public override List<Vector2> PreDecision(){
        Stage stage = GameManager.Instance.BattleManager.GetStage();
        return Gridlib.WayStraightBlockable(stage, stage.player.position, false);
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
        int damageCount = 0;
        int count = GameManager.Instance.CardManager.cardHand.Count - 1;
        while(Gridlib.InRange(stage, damagePos) && damageCount < count){
            Enemy damageEnemy = null;
            foreach(Enemy e in stage.enemies){
                if(e.position == damagePos) {
                    damageEnemy = e; 
                    break;
                }
            }
            if(damageEnemy != null){
                damageEnemy.GetDamage(GetValue());
                damageCount++;
                continue;
            }
            damagePos += nv;
        }
        
        List<Card> discardCards = new List<Card>();
        foreach(Card c in GameManager.Instance.CardManager.cardHand){
            if(c != GameManager.Instance.CardManager.GetSelectedCard()) discardCards.Add(c);
        }
        foreach(Card c in discardCards){
            GameManager.Instance.CardManager.DiscardCard(c, 2);    
        }

    }


}
