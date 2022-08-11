using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSmallUI : CardUI
{

    public new void UpdateCard()
    {
        spellinfo.GetValue();
        cardName.text = _name;
        cardCost.text = _cost.ToString();
    }
}
