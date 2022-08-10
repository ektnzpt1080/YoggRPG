using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardUI : MonoBehaviour
{
    public SpellInfo spellinfo {get;set;}

    string _name, _text;
    int _cost, _value;

    [SerializeField] public TextMeshProUGUI cardName, cardText, cardCost;

    public void Copy(SpellInfo _spellinfo){
        spellinfo = _spellinfo;
        _name = _spellinfo.name;
        _cost = _spellinfo.cost;
        _text = _spellinfo.text;
    }

    // Update is called once per frame
    void Update()
    {
        spellinfo.GetValue();
        cardName.text = _name;
        cardCost.text = _cost.ToString();
        cardText.text = _text.Replace("<V>", spellinfo.spell.value.ToString());
    }
}
