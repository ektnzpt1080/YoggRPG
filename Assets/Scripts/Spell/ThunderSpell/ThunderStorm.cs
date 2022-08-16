using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/ThunderStorm")]
public class ThunderStorm : Thunder
{
    public override void Decision(Vector2 selectedPos){
        thunderCount = 15;
        RandomTargeting(thunderCount);
    }
}
