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
    }

    CardType cardtype;
    public SpellInfo spellinfo {get;set;}
    [SerializeField] SpriteRenderer frontSpriteRenderer;
    [SerializeField] List<TextMeshPro> textSpriteRenderer;
    
    public PRS originPRS {get;set;}
    public int originOrder {get;set;}
    
    public string _name, _text;
    public int _cost, _value;
    
    public bool mouseInteractable {get;set;}

    [SerializeField] public TextMeshPro cardName, cardText, cardCost;

    public void Ordering(int order){
        foreach(TextMeshPro t in textSpriteRenderer){
            t.sortingOrder = order * 10 + 1;
        }
        frontSpriteRenderer.sortingOrder = order * 10;
    }
    
    //카드에 spell을 넣고, 준비시킴
    public void Copy(SpellInfo _spellinfo, CardType type){
        spellinfo = _spellinfo;
        _name = _spellinfo.name;
        _cost = _spellinfo.cost;
        _text = _spellinfo.text;
        cardtype = type;
        if(type == CardType.handCard) SetMouseInteractable(true);
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

    public void SetMouseInteractable(bool isInteractable){
        mouseInteractable = isInteractable;
    }

    public void Update(){
        spellinfo.GetValue();
        cardName.text = _name;
        cardCost.text = _cost.ToString();
        cardText.text = _text.Replace("<V>", spellinfo.spell.value.ToString());
    }
}

