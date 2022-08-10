using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Spell : ScriptableObject
{
    public int value;

    public abstract List<Vector2> PreDecision();
    public abstract List<Vector2> YoggDecision();
    public abstract void Decision(Vector2 selectedPos);
    
    public int GetValue(){
        return value;
    }

}
