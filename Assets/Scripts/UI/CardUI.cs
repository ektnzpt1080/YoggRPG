using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public SpellInfo spellinfo {get;set;}

    public enum CardUIType{
        deck,
        reward,
        yoggReward
    }
    public CardUIType type;
    public RewardCardCanvas rcc;

    protected string _name, _text;
    protected int _cost, _value;

    [SerializeField] public TextMeshProUGUI cardName, cardText, cardCost;

    public void Copy(SpellInfo _spellinfo, CardUIType _type){
        spellinfo = _spellinfo;
        _name = _spellinfo.name;
        _cost = _spellinfo.cost;
        _text = _spellinfo.text;
        type = _type;
    }

    // Update is called once per frame
    public void UpdateCard()
    {
        spellinfo.GetValue();
        cardName.text = _name;
        cardCost.text = _cost.ToString();
        cardText.text = _text.Replace("<V>", spellinfo.spell.value.ToString());
    }

    public void SetRewardCanvas(RewardCardCanvas r){
        rcc = r;
    }

    public void OnPointerClick(PointerEventData eventData){
        if(type == CardUIType.reward) rcc.PickReward(this);
    }
}
