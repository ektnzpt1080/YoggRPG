using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Spell : ScriptableObject
{
    public SpellInfo spellinfo;
    public int number; // 스펠 고유 넘버

    public abstract List<Vector2> PreDecision();
    public abstract List<Vector2> YoggDecision();
    public abstract void Decision(Vector2 selectedPos);
    public abstract void GetSpellInfo();
    
    public int GetValue(){
        Player p = GameManager.Instance.BattleManager.GetStage().player;
        return Mathf.FloorToInt(spellinfo.basic + spellinfo.strCoeff * p.strength + spellinfo.intCoeff * p.intelligence + spellinfo.healthCoeff * p.maxHealth);
    }

}
