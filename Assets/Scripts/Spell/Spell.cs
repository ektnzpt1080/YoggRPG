using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public SpellInfo spellinfo {get; set;}
    public int number {get; set;} // 스펠 고유 넘버
    Sprite sprite; // 스펠 스프라이트
    string text; // 스펠 설명

    public abstract List<Vector2> PreDecision();
    public abstract void Decision(Vector2 selectedPos);
    public abstract SpellInfo GetSpellInfo();
    
    public int GetValue(){
        Player p = GameManager.Instance.BattleManager.GetStage().player;
        return Mathf.FloorToInt(spellinfo.basic + spellinfo.strCoeff * p.strenghth + spellinfo.intCoeff * p.intelligence + spellinfo.healthCoeff * p.maxHealth);
    }

}
