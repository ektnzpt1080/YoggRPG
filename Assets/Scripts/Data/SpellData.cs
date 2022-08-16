using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpellInfo
{
    public string name;
    public int identitynumber;
    public Sprite sprite;
    public string text;
    public int cost;
    public int basic;
    public float strCoeff;
    public float intCoeff;
    public float healthCoeff;
    public int rarity; // 0 - 기본, 1 - 동, 2 - 은, 3 - 금, 4 - 요그 전용
    public Spell spell;

    public int GetValue(){
        Player p = GameManager.Instance.BattleManager.GetStage().player;
        return spell.value = Mathf.FloorToInt(basic + strCoeff * p.strength + intCoeff * p.intelligence + healthCoeff * p.maxHealth);
    }

    public SpellInfo Clone(){
        return (SpellInfo)this.MemberwiseClone();
    }
    
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellData", order = 3)]
public class SpellData : ScriptableObject
{
    public SpellInfo[] spellinfo;

    public SpellInfo FindSpell(int num){
        SpellInfo result = null;
        for(int i =  0; i < spellinfo.Length; i++){
            if(spellinfo[i].identitynumber == num) {
                result = spellinfo[i];
                break;
            }
        }
        return result;
    }

    public List<SpellInfo> GetSpellsWithRare(int rare){
        List<SpellInfo> result = new List<SpellInfo>();
        for(int i =  0; i < spellinfo.Length; i++){
            if(spellinfo[i].rarity == rare) {
                result.Add(spellinfo[i]);
            }
        }
        return result;
    }
}