using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/Reload")]
public class Reload : RevolverSpell
{
    public override List<Vector2> PreDecision(){
        return null;
    }
    public override List<Vector2> YoggDecision(){
        return PreDecision();
    }
    public override void Decision(Vector2 selectedPos){
        Card c = GameManager.Instance.CardManager.GetRandomCard();
        GameManager.Instance.CardManager.DiscardCard(c, 2);
        for(int i = 0 ; i < 3; i++){
            GameManager.Instance.CardManager.DrawCard();
        }
    }
    public override void Discarded(){
        GameManager.Instance.CardManager.DrawCard();
    }
}
