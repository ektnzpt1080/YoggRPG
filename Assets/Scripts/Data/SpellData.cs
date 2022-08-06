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
}
