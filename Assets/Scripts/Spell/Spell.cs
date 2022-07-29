using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    int number; // 스펠 고유 넘버
    Sprite sprite; // 스펠 스프라이트
    string text; // 스펠 설명

    public abstract List<Vector2> PreDecision();
    public abstract void Decision(Vector2 selectedPos);


}
