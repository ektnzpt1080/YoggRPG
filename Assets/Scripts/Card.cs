using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Card : MonoBehaviour
{
    public enum CardType{
        handCard = 0,
        yoggCard = 1,
        rewardCard = 2
    }

    CardType cardtype;
    public Spell spell {get;set;}
    [SerializeField] SpriteRenderer frontSpriteRenderer;
    [SerializeField] SpriteRenderer backSpriteRenderer;
    [SerializeField] List<TextMeshPro> textSpriteRenderer;
    
    public PRS originPRS {get;set;}
    public int originOrder {get;set;}
    
    string _name, _text;
    int _cost, _value;
    
    [SerializeField] public TextMeshPro cardName, cardText, cardCost;

    public void Ordering(int order){
        foreach(TextMeshPro t in textSpriteRenderer){
            t.sortingOrder = order * 10 + 1;
        }
        frontSpriteRenderer.sortingOrder = order * 10;
        backSpriteRenderer.sortingOrder = order * 10 - 1;
    }
    
    //카드에 spell을 넣고, 준비시킴
    public void Copy(Spell _spell, CardType type){
        spell = _spell;
        spell.GetSpellInfo();
        _name = spell.spellinfo.name;
        _cost = spell.spellinfo.cost;
        _text = spell.spellinfo.text;
        cardtype = type;
    }

    public void OnMouseOver(){
        if(cardtype == CardType.handCard){
            GameManager.Instance.CardManager.ChangeCardSize(this, true);
        }
    }

    public void OnMouseExit(){
        if(cardtype == CardType.handCard){
            GameManager.Instance.CardManager.ChangeCardSize(this, false);
        }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0f){
        if (useDotween)
        {
            transform.DOMove(prs.position, dotweenTime);
            transform.DORotateQuaternion(prs.rotation, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.position;
            transform.rotation = prs.rotation;
            transform.localScale = prs.scale;
        }
    }

    public Spell getSpell(){
        return spell;
    }

    public void Update(){
        _value = spell.GetValue();
        cardName.text = _name;
        cardCost.text = _cost.ToString();
        cardText.text = _text.Replace("<V>", _value.ToString());
    }

    
}

