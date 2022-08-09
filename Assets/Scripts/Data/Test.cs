using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]



[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellFFFF", order = 3)]
public class Test : ScriptableObject
{
    public Spell[] spellinfo;

}
